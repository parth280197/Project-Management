using Project_Management.Models;

namespace Project_Management.Helpers
{

  public class ProjectManagement
  {
    private ApplicationDbContext dbContext;
    public ProjectManagement(ApplicationDbContext dbContext)
    {
      this.dbContext = dbContext;
    }
    public bool CreateProject(Project project)
    {
      if (project != null)
      {
        dbContext.Projects.Add(project);
        dbContext.SaveChanges();
        return true;
      }
      return false;
    }
  }
}