using Project_Management.Models;

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
  }
}