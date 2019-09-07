using Project_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Management.Helpers
{
  public class NotificationManagement
  {
    private ApplicationDbContext db;
    public NotificationManagement(ApplicationDbContext db)
    {
      this.db = db;
    }
    public List<Notification> GetNotifications(User user)
    {
      var notifications = db.Notifications.Where(n => n.User.Id == user.Id && !n.IsOpened).ToList();

      return notifications;
    }
    public List<Notification> GetAllNotifications(User user)
    {
      var notifications = db.Notifications.Where(n => n.User.Id == user.Id).ToList();
      if (notifications.Count > 0)
      {
        MarkNotificationRead(notifications);
      }
      return notifications;
    }

    public void MarkNotificationRead(List<Notification> notifications)
    {
      foreach (var notification in notifications)
      {
        notification.IsOpened = true;
        db.SaveChanges();
      }
    }
    public void AddNotification(List<UserTask> tasks, string userId, NotificationType notificationType)
    {
      foreach (var task in tasks)
      {
        bool addFlag = true;
        var notifications = db.Notifications.ToList();
        foreach (var notification in notifications)
        {
          if (notification.Task.Id == task.Id && notification.User.Id == userId && notification.NotificationType == notificationType)
          {
            addFlag = false;
          }
        }
        if (addFlag)
        {
          var user = db.Users.Find(userId);
          Notification notification = new Notification()
          {
            Detail = task.Name + "is pending complete it before" + task.Deadline.ToString("d MMM yyyy"),
            IsOpened = false,
            Task = task,
            Project = task.Project,
            Time = DateTime.Now,
            User = user,
            NotificationType = notificationType,
          };

          db.Notifications.Add(notification);
          db.SaveChanges();
        }
      }
    }

    public void AddCompletedNotification(UserTask task, NotificationType notificationType)
    {
      var projectManagers = db.Users.Where(u => u.PersonType == PersonType.ProjectManager).ToList();
      bool addFlag = true;

      foreach (var projectManager in projectManagers)
      {
        var notifications = db.Notifications.ToList();
        foreach (var notification in notifications)
        {
          if (notification.Task.Id == task.Id && notification.User.Id == projectManager.Id && notification.NotificationType == notificationType)
          {
            addFlag = false;
          }
        }
        if (addFlag)
        {
          Notification notification = new Notification()
          {
            Detail = task.Name + " is completed on " + DateTime.Now.ToString("d MMM yyyy"),
            IsOpened = false,
            Task = task,
            Project = task.Project,
            Time = DateTime.Now,
            User = projectManager,
            NotificationType = notificationType,
          };

          db.Notifications.Add(notification);
          db.SaveChanges();
        }
      }
    }
  }
}