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

    #region Get methods
    /// <summary>
    /// Gives all unopened notification for specific user.
    /// </summary>
    /// <param name="user">To get all unopened notification of the user</param>
    /// <returns>List of all unopend notification</returns>
    public List<Notification> GetNotifications(User user)
    {
      var notifications = db.Notifications.Where(n => n.User.Id == user.Id && !n.IsOpened).ToList();

      return notifications;
    }

    /// <summary>
    /// Gives all notification for specific user.
    /// </summary>
    /// <param name="user">To get all notification of the user</param>
    /// <returns>List of all notification</returns>
    public List<Notification> GetAllNotifications(User user)
    {
      var notifications = db.Notifications.Where(n => n.User.Id == user.Id).ToList();
      if (notifications.Count > 0)
      {
        MarkNotificationRead(notifications);
      }
      return notifications;
    }
    #endregion

    #region Create methods

    /// <summary>
    /// Add pending notification for perticular task in database.
    /// </summary>
    /// <param name="tasks">To add taskId in notification from Task.</param>
    /// <param name="userId">To add userId in notification from Task.</param>
    /// <param name="notificationType">Specifies type of notification.</param>
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

    /// <summary>
    /// Add completed notification for perticular task in database.
    /// </summary>
    /// <param name="task">To add taskId in notification from Task.</param>
    /// <param name="notificationType">Specifies type of notification.</param>
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
    #endregion

    #region Update method
    /// <summary>
    /// Mark notification as read in database.
    /// </summary>
    /// <param name="notifications">List of notification to be marked as read or opened.</param>
    public void MarkNotificationRead(List<Notification> notifications)
    {
      foreach (var notification in notifications)
      {
        notification.IsOpened = true;
        db.SaveChanges();
      }
    }
    #endregion
  }
}