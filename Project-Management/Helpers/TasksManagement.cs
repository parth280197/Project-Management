using Project_Management.Models;
using Project_Management.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Project_Management.Helpers
{
  public class TasksManagement
  {
    private ApplicationDbContext db;
    public TasksManagement()
    {
      db = new ApplicationDbContext();
    }
    public bool CreateTask(UserTaskFormViewModel userTaskFormView)
    {
      var task = userTaskFormView.Task;
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
        return true;
      }
      return false;
    }
    public bool UpdateTask(UserTaskFormViewModel userTaskFormView)
    {
      var task = userTaskFormView.Task;
      var selectedUsers = userTaskFormView.SelectedId;
      if (task != null)
      {
        var taskInDb = db.Tasks.Find(task.Id);
        bool updateFlag = false;
        taskInDb.Name = task.Name;
        taskInDb.ProjectId = task.ProjectId;
        taskInDb.CompletedPercentage = task.CompletedPercentage;
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
          CompletedPercentage = task.CompletedPercentage
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
  }
}