using Project_Management.Models;
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
    public bool CreateTask(UserTask userTask, string[] developers)
    {
      if (userTask != null)
      {
        db.Tasks.Add(userTask);
        db.SaveChanges();
        foreach (string developerId in developers)
        {
          var user = db.Users.Find(developerId);
          userTask.Users.Add(user);
          db.SaveChanges();
        }
        return true;
      }
      return false;
    }
    public bool UpdateTask(UserTask userTask, string[] developers)
    {
      if (userTask != null)
      {
        var taskInDb = db.Tasks.Find(userTask.Id);
        taskInDb.Name = userTask.Name;
        taskInDb.ProjectId = userTask.ProjectId;
        taskInDb.CompletedPercentage = userTask.CompletedPercentage;
        List<User> users = new List<User>();
        foreach (string developerId in developers)
        {
          var user = db.Users.Find(developerId);
          if (!taskInDb.Users.Contains(user))
          {
            users.Add(user);
          }

        }
        taskInDb.Users = users;
        db.SaveChanges();
        return true;
      }
      return false;
    }
  }
}