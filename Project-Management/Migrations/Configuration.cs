namespace Project_Management.Migrations
{
  using Project_Management.Helpers;
  using Project_Management.Models;
  using System.Data.Entity.Migrations;

  internal sealed class Configuration : DbMigrationsConfiguration<Project_Management.Models.ApplicationDbContext>
  {
    private RoleManagement roleManagement;
    private ApplicationDbContext dbContext;
    public Configuration()
    {
      dbContext = new ApplicationDbContext();
      roleManagement = new RoleManagement(dbContext);
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(Project_Management.Models.ApplicationDbContext context)
    {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data.
      roleManagement.CreateRole("Developer");
      roleManagement.CreateRole("Admin");

    }
  }
}
