using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Project_Management.ViewModels
{
  public class UserTaskFormViewModel
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [Display(Name = "Completed Work in %")]
    public double CompletedPercentage { get; set; }
    [Required]
    public int ProjectId { get; set; }
    public IEnumerable<SelectListItem> UsersList { get; set; }
    public string[] SelectedId { get; set; }
  }
}