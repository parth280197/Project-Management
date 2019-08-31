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
      return View(db.Projects.Find(id).Tasks.ToList());
    }
    public ActionResult CreateOrUpdate(int id)
    {
      UserTaskFormViewModel userTaskFormViewModel = new ViewModels.UserTaskFormViewModel();
      userTaskFormViewModel.ProjectId = id;
      //userTaskFormViewModel.Users = db.Users.ToList();
      return View(userTaskFormViewModel);
    }
    [HttpPost]
    public ActionResult CreateOrUpdate(UserTaskFormViewModel userTaskFormViewModel, string[] Developers)
    {
      if (ModelState.IsValid)
      {
        UserTask userTask = new UserTask()
        {
          Name = userTaskFormViewModel.Name,
          Description = userTaskFormViewModel.Description,
          ProjectId = userTaskFormViewModel.ProjectId,
          CompletedPercentage = userTaskFormViewModel.CompletedPercentage
        };
        tasksManagement.CreateTask(userTask, Developers);

      }
      return View(userTaskFormViewModel);
    }
  }
}