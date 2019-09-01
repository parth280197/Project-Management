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
    [Display(Name = "Select developers for the Task.")]
    public IEnumerable<SelectListItem> UsersList { get; set; }
    [Required(ErrorMessage = "*Please select developer. \n *To select multiple developer hold ctrl key.")]

    public string[] SelectedId { get; set; }
  }
}