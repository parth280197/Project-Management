﻿using Project_Management.Models;
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
        taskInDb.Deadline = task.Deadline;
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
        CompletedPercentage = task.CompletedPercentage,
        Priority = task.Priority,
        Deadline = task.Deadline,
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
    public bool CheckDeadlines(string userId)
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
        notificationTasks = user.Tasks.Where(t => (tommorowDate.Day - t.Deadline.Day) <= 1).ToList();
      }

      notificationManagement.AddNotification(notificationTasks, userId, NotificationType.Incompleted);
      return true;
    }
  }
}
