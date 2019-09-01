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
        projectIndb.Name = project.Name;
        projectIndb.Description = project.Description;
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
  }
}