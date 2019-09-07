using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_Management.Models;

namespace Project_Management.Helpers
{

  public class RoleManagement
  {
    ApplicationDbContext db;
    RoleManager<IdentityRole> roleManager;
    public RoleManagement(ApplicationDbContext db)
    {
      this.db = db;
      roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
    }

    /// <summary>
    /// Create new role in database.
    /// </summary>
    /// <param name="roleName">Name of role to be added in database.</param>
    /// <returns></returns>
    public bool CreateRole(string roleName)
    {
      return roleManager.Create(new IdentityRole(roleName)).Succeeded;
    }
  }
}