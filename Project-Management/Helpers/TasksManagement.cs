using Project_Management.Models;
using Project_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Project_Management.Helpers
{
  public class TasksManagement
  {
    private ApplicationDbContext db;
    private ProjectManagement projectManagement;
    private NotificationManagement notificationManagement;

    public TasksManagement(ApplicationDbContext db)
    {
      this.db = db;
      projectManagement = new ProjectManagement(db);
      notificationManagement = new NotificationManagement(db);
    }

    #region Get functions
    /// <summary>
    /// GetUserTasks gives list of Task for perticular User involved in perticular project.
    /// </summary>
    /// <param name="projectId">for to search user task in that project.</param>
    /// <param name="userId">UserId to perform search for task in given project.</param>
    /// <returns>List of Tasks for specific user involved in specific project. </returns>
    public List<UserTask> GetUserTasks(int projectId, string userId)
    {
      var user = db.Users.Find(userId);
      var project = db.Projects.Find(projectId);

      var userTasks = new List<UserTask>();
      foreach (UserTask task in project.Tasks)
      {
        if (task.Users.Contains(user))
        {
          userTasks.Add(task);
        }
      }

      return userTasks.ToList();
    }

    /// <summary>
    /// gives Task according to taskId.
    /// </summary>
    /// <param name="taskId">unique taskId to get specific task from database.</param>
    /// <returns>Task which has provided unique taskId</returns>
    public UserTask GetUserTask(int taskId)
    {
      var task = db.Tasks.Find(taskId);
      return task;
    }

    /// <summary>
    /// Will load the specific task from database and convert it to the <c>UserTaskFormViewModel</c>.
    /// </summary>
    /// <param name="taskId">to get perticular task from database.</param>
    /// <returns>Model for the view.</returns>
    public UserTaskFormViewModel LoadViewModel(int taskId)
    {
      var task = db.Tasks.Find(taskId);
      List<string> selectedUsers = new List<string>();
      selectedUsers = task.Users.Select(u => u.Id).ToList();
      UserTaskFormViewModel userTaskFormViewModel = new UserTaskFormViewModel()
      {
        Task = new UserTask()
        {
          Id = task.Id,
          Name = task.Name,
          Description = task.Description,
          ProjectId = task.ProjectId,
          CompletedPercentage = task.CompletedPercentage,
          Deadline = task.Deadline,
          Priority = task.Priority,
        },
        UsersList = db.Users.Where(u => u.PersonType == PersonType.Developer)
        .Select(u => new SelectListItem
        {
          Text = u.Name,
          Value = u.Id
        }),
        SelectedId = selectedUsers.ToArray(),
      };
      return userTaskFormViewModel;
    }

    /// <summary>
    /// Will load the specific task from database and convert it to the <c>DevTaskViewModel</c>.
    /// </summary>
    /// <param name="taskId">to get perticular task from database.</param>
    /// <returns>Model for the view.</returns>
    public DevTaskViewModel LoadDevViewModel(int taskId)
    {
      var task = db.Tasks.Find(taskId);

      DevTaskViewModel devTaskViewModel = new DevTaskViewModel()
      {
        Id = task.Id,
        Name = task.Name,
        Description = task.Description,
        ProjectId = task.ProjectId,
        CompletedPercentage = task.CompletedPercentage,
        Priority = task.Priority,
        Deadline = task.Deadline,
      };

      return devTaskViewModel;
    }

    #endregion

    #region Create and Update functions
    /// <summary>
    /// Collect task, project, and selected users from <c>UserTaskFormViewModel</c> and create new record in database. 
    /// </summary>
    /// <param name="userTaskFormView">to get required task, project, and selected users from ViewModel to make record in database.</param>
    public void CreateTask(UserTaskFormViewModel userTaskFormView)
    {
      var task = userTaskFormView.Task;
      task.Project = db.Projects.Find(task.ProjectId);
      var selectedUsers = userTaskFormView.SelectedId;
      if (userTaskFormView.Task != null)
      {
        db.Tasks.Add(task);
        db.SaveChanges();
        foreach (string developerId in selectedUsers)
        {
          var user = db.Users.Find(developerId);
          task.Users.Add(user);
          db.SaveChanges();
        }
        projectManagement.UpdateCompletedWork(task.Project);
      }
    }

    /// <summary>
    /// Collect task, project, and selected users from <c>UserTaskFormViewModel</c> and update existing Task record in database.
    /// </summary>
    /// <param name="userTaskFormView">to get required task, project, and selected users from ViewModel to update record in database.</param>
    public void UpdateTask(UserTaskFormViewModel userTaskFormView)
    {
      var task = userTaskFormView.Task;
      task.Project = db.Projects.Find(task.ProjectId);
      var selectedUsersId = userTaskFormView.SelectedId;

      if (task != null)
      {
        var taskInDb = db.Tasks.Find(task.Id);

        taskInDb.Name = task.Name;
        taskInDb.ProjectId = task.ProjectId;
        taskInDb.CompletedPercentage = task.CompletedPercentage;
        taskInDb.Deadline = task.Deadline;
        taskInDb.Priority = task.Priority;

        List<User> selectedUsers = new List<User>();
        //Get all selected users from view model
        foreach (string developerId in selectedUsersId)
        {
          var user = db.Users.Find(developerId);
          selectedUsers.Add(user);
        }

        //Get all users to remove in removed list
        List<User> removedUsers = new List<User>();
        foreach (User user in taskInDb.Users)
        {
          if (!selectedUsers.Contains(user))
          {
            removedUsers.Add(user);
          }
        }

        //Remove it from task
        foreach (var user in removedUsers)
        {
          taskInDb.Users.Remove(user);
          db.SaveChanges();
        }


        //add all new user selected from view model
        foreach (var user in selectedUsers)
        {
          if (!taskInDb.Users.Contains(user))
          {
            taskInDb.Users.Add(user);
          }
        }
        db.SaveChanges();

        //Update completed % of work in project 
        projectManagement.UpdateCompletedWork(taskInDb.Project);

        if (task.CompletedPercentage == 100)
        {
          //if task completed add notification
          notificationManagement.AddCompletedNotification(taskInDb, NotificationType.Completed);
        }
      }
    }

    /// <summary>
    /// Collect CompletedPercentage of task, notes from <c>DevTaskViewModel</c> and update existing task record in database.
    /// </summary>
    /// <param name="devTaskViewModel">to get required CompletedPercentage of task, notes from ViewModel to update record in database.</param>
    /// <param name="userId">to make record in notes table in database.</param>
    public void DevUpdateTask(DevTaskViewModel devTaskViewModel, string userId)
    {
      var taskInDb = db.Tasks.Find(devTaskViewModel.Id);
      taskInDb.CompletedPercentage = devTaskViewModel.CompletedPercentage;

      var note = new Notes()
      {
        Comment = devTaskViewModel.Note,
        CreatedDate = DateTime.Now,
        User = db.Users.Find(userId)
      };
      taskInDb.Notes.Add(note);
      db.SaveChanges();
      projectManagement.UpdateCompletedWork(taskInDb.Project);
      if (devTaskViewModel.CompletedPercentage == 100)
      {
        notificationManagement.AddCompletedNotification(taskInDb, NotificationType.Completed);
      }
    }
    #endregion

    #region Delete functions
    /// <summary>
    ///   Delete the all the notes, notification associated with the task and remove task it-self.
    /// </summary>
    /// <param name="taskId">to get specific task to remove from database.</param>
    public void DeleteTask(int taskId)
    {
      var task = db.Tasks.Find(taskId);
      var project = task.Project;

      db.Notes.RemoveRange(task.Notes);

      var notificationsToRemove = db.Notifications.Where(n => n.Task.Id == task.Id).ToList();
      db.Notifications.RemoveRange(notificationsToRemove);

      db.Tasks.Remove(task);

      db.SaveChanges();

      projectManagement.UpdateCompletedWork(project);
    }
    #endregion

    #region Notification functions
    /// <summary>
    /// will check for incomplete task deadline and add notification to specific user.
    /// </summary>
    /// <param name="userId">to get user and associated required information from database.</param>
    public void CheckDeadlines(string userId)
    {
      var user = db.Users.Find(userId);
      var notificationTasks = new List<UserTask>();

      if (user.PersonType == PersonType.ProjectManager)
      {
        //get list of the tasks that are not finished yet and passed their deadline. 
        notificationTasks = db.Tasks.Where(t => t.Deadline < DateTime.Now && t.CompletedPercentage < 100).ToList();
      }
      else
      {
        //get all task with difference between tommorow date and deadline is 1 or lessthen 1.
        DateTime tommorowDate = DateTime.Now.AddDays(1);
        notificationTasks = user.Tasks.Where(t => (t.Deadline.Day - tommorowDate.Day) <= 1).ToList();
      }

      notificationManagement.AddNotification(notificationTasks, userId, NotificationType.Incompleted);
    }
    #endregion
  }
}
