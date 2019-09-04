using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_Management.Models;

namespace Project_Management.Helpers
{
  public class UserManagement
  {
    ApplicationDbContext db;
    UserManager<User> userManager;
    public UserManagement(ApplicationDbContext db)
    {
      this.db = db;
      userManager = new UserManager<User>(new UserStore<User>(db));
    }
    public bool AddUserToRole(string UserId, string RoleName)
    {
      return userManager.AddToRole(UserId, RoleName).Succeeded;
    }
    public bool UserInRole(string userId, string roleName)
    {
      return userManager.IsInRole(userId, roleName);
    }
  }
}