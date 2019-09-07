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

    /// <summary>
    /// This function assign the Role to the provided user.
    /// </summary>
    /// <param name="userId">UserId for user.</param>
    /// <param name="roleName">RoleName to assign user to that rule.</param>
    /// <returns></returns>
    public bool AddUserToRole(string userId, string roleName)
    {
      return userManager.AddToRole(userId, roleName).Succeeded;
    }
    /// <summary>
    /// Check whether User exist in a provided role name or not
    /// </summary>
    /// <param name="userId">UserId for user.</param>
    /// <param name="roleName">RoleName to check whether user is in role or not.</param>
    /// <returns>true if User assigned to that perticular role.</returns>
    public bool UserInRole(string userId, string roleName)
    {
      return userManager.IsInRole(userId, roleName);
    }
  }
}