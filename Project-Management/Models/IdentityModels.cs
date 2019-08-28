using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_Management.Models
{
  // You can add profile data for the user by adding more properties to your User class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class User : IdentityUser
  {
    public string Name { get; set; }
    public PersonType PersonType { get; set; }
    public virtual ICollection<UserTask> Tasks { get; set; }
    public virtual ICollection<Notes> Notes { get; set; }
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }
  }
  public class Project
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual User CreatedBy { get; set; }
    public double CompletedPercentage { get; set; }
    public virtual ICollection<UserTask> Tasks { get; set; }
  }

  public class UserTask
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double CompletedPercentage { get; set; }
    public virtual ICollection<User> Users { get; set; }
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
    public virtual ICollection<Notes> Notes { get; set; }
  }

  public class Notes
  {
    public int Id { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual UserTask UserTask { get; set; }
    public virtual User User { get; set; }

  }
  public enum PersonType
  {
    ProjectManager,
    Developer,
  }
  public class ApplicationDbContext : IdentityDbContext<User>
  {
    public DbSet<Project> Projects { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
    {
    }

    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }
  }
}