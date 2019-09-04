using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_Management.Models
{
  // You can add profile data for the user by adding more properties to your User class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class User : IdentityUser
  {
    public static User Identity { get; internal set; }
    [Required]
    public string Name { get; set; }
    [Required]
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
    public Project()
    {
      Tasks = new HashSet<UserTask>();
    }
    public int Id { get; set; }
    [Required]
    [Display(Name = "Project name")]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public virtual User CreatedBy { get; set; }
    [Required]
    [Display(Name = "Work completed in %")]
    [Range(0, 100)]
    public double CompletedPercentage { get; set; }
    [Required]
    public Priority Priority { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]

    public DateTime Deadline { get; set; }
    public virtual ICollection<UserTask> Tasks { get; set; }
  }

  public class UserTask
  {
    public UserTask()
    {
      Users = new HashSet<User>();
    }
    [Required]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Task name")]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [Display(Name = "Work completed in %")]
    [Range(0, 100)]
    public double CompletedPercentage { get; set; }
    [Required]
    public Priority Priority { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]

    public DateTime Deadline { get; set; }
    public virtual ICollection<User> Users { get; set; }
    [Required]
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
    public virtual ICollection<Notes> Notes { get; set; }
  }

  public class Notes
  {
    public int Id { get; set; }
    [Required]
    public string Comment { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
    public virtual UserTask UserTask { get; set; }
    public virtual User User { get; set; }

  }
  public class Notification
  {
    public int Id { get; set; }
    public string Detail { get; set; }
    public virtual User User { get; set; }
    public virtual UserTask Task { get; set; }
    public virtual Project Project { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
    public DateTime Time { get; set; }
    public bool IsOpened { get; set; }
  }
  public enum PersonType
  {
    ProjectManager,
    Developer,
  }

  public enum Priority
  {
    Low,
    Normal,
    High,
  }
  public class ApplicationDbContext : IdentityDbContext<User>
  {
    public DbSet<Project> Projects { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Notes> Notes { get; set; }
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