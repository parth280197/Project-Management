using Project_Management.Helpers;
using Project_Management.Models;
using Project_Management.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
  public class TasksController : Controller
  {
    private ApplicationDbContext db;
    private TasksManagement tasksManagement;
    public TasksController()
    {
      db = new ApplicationDbContext();
      tasksManagement = new TasksManagement();
    }
    // GET: Tasks
    public ActionResult List(int id)
    {
      ViewBag.ProjectId = id;
      return View(db.Projects.Find(id).Tasks.ToList());
    }
    public ActionResult CreateOrUpdate(int projectId)
    {
      UserTaskFormViewModel userTaskFormViewModel = new ViewModels.UserTaskFormViewModel();
      userTaskFormViewModel.Task.ProjectId = projectId;
      userTaskFormViewModel.UsersList = db.Users.Where(u => u.PersonType == PersonType.Developer)
        .Select(u => new SelectListItem
        {
          Text = u.Name,
          Value = u.Id
        });
      userTaskFormViewModel.SelectedId = new string[] { };
      return View("TasksForm", userTaskFormViewModel);
    }
    [HttpPost]
    public ActionResult CreateOrUpdate(UserTaskFormViewModel userTaskFormViewModel)
    {
      if (ModelState.IsValid)
      {
        if (userTaskFormViewModel.Task.Id == 0)
        {
          UserTask userTask = new UserTask()
          {
            Name = userTaskFormViewModel.Task.Name,
            Description = userTaskFormViewModel.Task.Description,
            ProjectId = userTaskFormViewModel.Task.ProjectId,
            CompletedPercentage = userTaskFormViewModel.Task.CompletedPercentage
          };
          tasksManagement.CreateTask(userTask, userTaskFormViewModel.SelectedId);
        }
        else
        {
          UserTask userTask = new UserTask()
          {
            Id = userTaskFormViewModel.Task.Id,
            Name = userTaskFormViewModel.Task.Name,
            Description = userTaskFormViewModel.Task.Description,
            ProjectId = userTaskFormViewModel.Task.ProjectId,
            CompletedPercentage = userTaskFormViewModel.Task.CompletedPercentage
          };
          tasksManagement.UpdateTask(userTask, userTaskFormViewModel.SelectedId);
        }
      }
      return RedirectToAction("List", new { id = userTaskFormViewModel.Task.ProjectId });
    }
    public ActionResult Edit(int id)
    {
      var task = db.Tasks.Find(id);
      List<string> selectedUsers = new List<string>();
      selectedUsers = task.Users.Select(u => u.Id).ToList();
      UserTaskFormViewModel userTaskFormViewModel = new UserTaskFormViewModel()
      {
        Task = new UserTask()
        {
          Id = task.Id,
          Name = task.Name,
          Description = task.Description,
          ProjectId = task.ProjectId,
          CompletedPercentage = task.CompletedPercentage
        },
        UsersList = db.Users.Where(u => u.PersonType == PersonType.Developer)
        .Select(u => new SelectListItem
        {
          Text = u.Name,
          Value = u.Id
        }),
        SelectedId = selectedUsers.ToArray(),
      };

      return View("TasksForm", userTaskFormViewModel);
    }
  }
}