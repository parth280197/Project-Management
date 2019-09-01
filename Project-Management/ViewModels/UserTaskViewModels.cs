using Project_Management.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Project_Management.ViewModels
{
  public class UserTaskFormViewModel
  {
    public UserTaskFormViewModel()
    {
      Task = new UserTask();
    }
    public UserTask Task { get; set; }
    public IEnumerable<SelectListItem> UsersList { get; set; }
    [Required]
    public string[] SelectedId { get; set; }
  }
}