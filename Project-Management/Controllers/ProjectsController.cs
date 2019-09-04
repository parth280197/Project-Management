using Microsoft.AspNet.Identity;
using Project_Management.Helpers;
using Project_Management.Models;
using System.Linq;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
  public class ProjectsController : Controller
  {
    private ProjectManagement projectManagement;
    private TasksManagement tasksManagement;
    private ApplicationDbContext db;
    public ProjectsController()
    {
      db = new ApplicationDbContext();
      projectManagement = new ProjectManagement(db);
      tasksManagement = new TasksManagement(db);
    }
    // GET: Projects
    [Authorize(Roles = "ProjectManager")]
    [HttpGet]
    [Authorize(Roles = "ProjectManager")]
    public ActionResult Create()
    {
      ViewBag.Action = "Create";
      return View("ProjectForm", new Project());
    }
    [Authorize(Roles = "ProjectManager")]
    public ActionResult CreateOrUpdate(Project project)
    {
      if (ModelState.IsValid)
      {
        if (project.Id == 0)
        {
          project.CompletedPercentage = 0;
          project.CreatedBy = db.Users.Find(User.Identity.GetUserId());
          projectManagement.CreateProject(project);
        }
        else
        {
          projectManagement.UpdateProject(project);
        }
      }
      else
      {
        return RedirectToAction("List");
      }
      return RedirectToAction("List");
    }
    [Authorize(Roles = "ProjectManager")]
    public ActionResult Edit(int projectId)
    {
      ViewBag.Action = "Update";
      var project = db.Projects.Find(projectId);
      return View("ProjectForm", project);
    }

    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult List()
    {
      //CheckDeadline checks for those tasks whoes deadline is tommorow and add it in notification table if previously not added.
      tasksManagement.CheckDeadlines(User.Identity.GetUserId());
      if (User.IsInRole("ProjectManager"))
      {
        return View(db.Projects.OrderByDescending(p => p.CompletedPercentage).ToList());
      }

      return View(projectManagement.GetUsersProject(db.Users.Find(User.Identity.GetUserId())).OrderByDescending(p => p.CompletedPercentage));

    }

    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult ListByPriority()
    {
      if (User.IsInRole("ProjectManager"))
      {
        return View("List", db.Projects.OrderByDescending(p => p.Priority).ToList());
      }

      return View("List", projectManagement.GetUsersProject(db.Users.Find(User.Identity.GetUserId())).OrderByDescending(p => p.Priority));

    }
  }
}