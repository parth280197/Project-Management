using Project_Management.Models;
using Project_Management.ViewModels;
using System.Collections.Generic;

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
  }
}