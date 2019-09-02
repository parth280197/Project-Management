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
    public TasksManagement()
    {
      db = new ApplicationDbContext();
      projectManagement = new ProjectManagement(db);
    }
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
      return userTasks.OrderByDescending(t => t.CompletedPercentage).ToList();
    }
    public bool CreateTask(UserTaskFormViewModel userTaskFormView)
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
        return true;
      }
      return false;
    }
    public bool UpdateTask(UserTaskFormViewModel userTaskFormView)
    {
      var task = userTaskFormView.Task;
      task.Project = db.Projects.Find(task.ProjectId);
      var selectedUsers = userTaskFormView.SelectedId;
      if (task != null)
      {
        var taskInDb = db.Tasks.Find(task.Id);
        bool updateFlag = false;
        taskInDb.Name = task.Name;
        taskInDb.ProjectId = task.ProjectId;
        taskInDb.CompletedPercentage = task.CompletedPercentage;
        taskInDb.Priority = task.Priority;
        List<User> users = new List<User>();
        foreach (string developerId in selectedUsers)
        {
          var user = db.Users.Find(developerId);
          users.Add(user);
        }
        foreach (User user in taskInDb.Users)
        {
          if (!users.Contains(user))
          {
            users.Remove(user);
            updateFlag = true;
          }
        }
        foreach (string developerId in selectedUsers)
        {
          var user = db.Users.Find(developerId);
          if (!taskInDb.Users.Contains(user))
          {
            users.Add(user);
            updateFlag = true;
          }
        }
        if (updateFlag)
        {
          taskInDb.Users = users;
        }
        db.SaveChanges();
        projectManagement.UpdateCompletedWork(task.Project);
        return true;
      }
      return false;
    }
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
    public DevTaskViewModel LoadDevViewModel(int taskId)
    {
      var task = db.Tasks.Find(taskId);
      DevTaskViewModel devTaskViewModel = new DevTaskViewModel()
      {
        Id = task.Id,
        Name = task.Name,
        Description = task.Description,
        ProjectId = task.ProjectId,
        CompletedPercentage = task.CompletedPercentage
      };
      return devTaskViewModel;
    }

    public bool DevUpdateTask(DevTaskViewModel devTaskViewModel, string userId)
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
      return false;
    }
    public UserTask GetUserTask(int taskId)
    {
      var task = db.Tasks.Find(taskId);
      return task;
    }
  }
}
