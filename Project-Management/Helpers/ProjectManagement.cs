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

    #region Get methods
    /// <summary>
    /// Gives the list of projects for specific user.
    /// </summary>
    /// <param name="user">to get projects in which provided user is involved.</param>
    /// <returns>List of project in which provided user is involved.</returns>
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
    #endregion

    #region Create and update methods

    /// <summary>
    /// Create new project in database
    /// </summary>
    /// <param name="project"> Ptoject to be added in the database.</param>
    public void CreateProject(Project project)
    {
      if (project != null)
      {
        db.Projects.Add(project);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// Update the existing project data for project manager
    /// </summary>
    /// <param name="project">Project to be updated in database</param>
    public void UpdateProject(Project project)
    {
      var projectIndb = db.Projects.Find(project.Id);

      if (projectIndb != null)
      {
        projectIndb.Priority = project.Priority;
        projectIndb.Name = project.Name;
        projectIndb.Description = project.Description;
        projectIndb.Deadline = project.Deadline;
        db.SaveChanges();
      }
    }

    /// <summary>
    /// Calculate and update project work % according to average of completed work of all tasks for that specific project.
    /// </summary>
    /// <param name="project">To update completed work % in that project</param>
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
    #endregion

    #region Delete methods
    /// <summary>
    /// Delete specified project and all relevant associated data to that project like tasks, notifications and notes.
    /// </summary>
    /// <param name="projectId">Id of Project to be removed.</param>
    public void DeleteProject(int projectId)
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
    }
    #endregion
  }
}