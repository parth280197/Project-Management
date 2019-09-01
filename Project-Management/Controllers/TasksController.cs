using Project_Management.Helpers;
using Project_Management.Models;
using Project_Management.ViewModels;
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
      return View(db.Projects.Find(id).Tasks.OrderBy(t => t.CompletedPercentage).ToList());
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
      return View("TasksForm", userTaskFormViewModel);
    }
    [HttpPost]
    public ActionResult CreateOrUpdate(UserTaskFormViewModel userTaskFormViewModel)
    {
      if (ModelState.IsValid)
      {
        if (userTaskFormViewModel.Task.Id == 0)
        {
          tasksManagement.CreateTask(userTaskFormViewModel);
        }
        else
        {
          tasksManagement.UpdateTask(userTaskFormViewModel);
        }
      }
      return RedirectToAction("List", new { id = userTaskFormViewModel.Task.ProjectId });
    }
    public ActionResult Edit(int id)
    {
      return View("TasksForm", tasksManagement.LoadViewModel(id));
    }
  }
}