using Project_Management.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Management.ViewModels
{
  public class DevTaskViewModel
  {
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
    public virtual ICollection<User> Users { get; set; }
    [Required]
    public int ProjectId { get; set; }
    [Required]
    public Priority Priority { get; set; }
    [Required]
    public string Note { get; set; }
  }
}