﻿using Project_Management.Models;
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
    public void AddNotification(List<UserTask> tasks)
    {
      foreach (var task in tasks)
      {
        bool addFlag = true;
        foreach (var notification in db.Notifications.ToList())
        {
          if (notification.Task.Id == task.Id)
          {
            addFlag = false;
          }
        }
        if (addFlag)
        {
          var user = db.Users.Find(User.Identity.Id);
          Notification notification = new Notification()
          {
            Detail = task.Name + "is pending complete it before" + task.Deadline,
            IsOpened = false,
            Task = task,
            Project = task.Project,
            Time = DateTime.Now,
            User = User.Identity,
          };

          db.Notifications.Add(notification);
          db.SaveChanges();
        }
      }
    }
  }
}