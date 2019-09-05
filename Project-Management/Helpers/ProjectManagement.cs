using Project_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Management.Helpers
{

  public class ProjectManagement
  {
    private ApplicationDbContext db;
    public ProjectManagement(ApplicationDbContext db)
    {
      this.db = db;
    }
    public bool CreateProject(Project project)
    {
      if (project != null)
      {
        db.Projects.Add(project);
        db.SaveChanges();
        return true;
      }
      return false;
    }
    public bool UpdateProject(Project project)
    {
      var projectIndb = db.Projects.Find(project.Id);
      if (projectIndb != null)
      {
        projectIndb.Priority = project.Priority;
        projectIndb.Name = project.Name;
        projectIndb.Description = project.Description;
        projectIndb.Deadline = project.Deadline;
        db.SaveChanges();
        return true;
      }
      return false;
    }
    public void UpdateCompletedWork(Project project)
    {
      double completedWork = 0;
      foreach (UserTask task in project.Tasks)
      {
        completedWork += task.CompletedPercentage;
      }
      completedWork = completedWork / project.Tasks.Count;
      project.CompletedPercentage = Math.Round(completedWork);
      db.SaveChanges();
    }
    public List<Project> GetUsersProject(User user)
    {
      var projects = new HashSet<Project>();
      foreach (UserTask task in db.Tasks)
      {
        if (task.Users.Contains(user))
        {
          projects.Add(task.Project);
        }
      }
      return projects.ToList();
    }
    public bool DeleteProject(int projectId)
    {
      var project = db.Projects.Find(projectId);
      var taskInProject = project.Tasks.ToList();
      foreach (var task in taskInProject)
      {
        db.Notes.RemoveRange(task.Notes);
        db.Tasks.Remove(task);

        var notificationsToRemove = db.Notifications.Where(n => n.Task.Id == task.Id).ToList();
        db.Notifications.RemoveRange(notificationsToRemove);
      }
      db.Projects.Remove(project);
      db.SaveChanges();
      return true;
    }
  }
}