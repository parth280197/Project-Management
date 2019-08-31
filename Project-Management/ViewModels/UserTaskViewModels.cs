using Project_Management.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Project_Management.ViewModels
{
  public class UserTaskFormViewModel
  {
    public UserTaskFormViewModel()
    {
      ApplicationDbContext db = new ApplicationDbContext();
      Users = db.Users.ToList();
    }
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [Display(Name = "Completed Work in %")]
    public double CompletedPercentage { get; set; }
    [Required]
    [Display(Name = "Developers")]
    public virtual ICollection<User> Users { get; set; }
    [Required]
    public int ProjectId { get; set; }

  }
}