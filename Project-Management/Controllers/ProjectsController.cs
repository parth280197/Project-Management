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
    private ApplicationDbContext db;
    public ProjectsController()
    {
      db = new ApplicationDbContext();
      projectManagement = new ProjectManagement(db);
    }
    // GET: Projects
    [Authorize(Roles = "ProjectManager")]
    [HttpGet]
    public ActionResult Create()
    {
      return View("ProjectForm", new Project());
    }
    [HttpPost]
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
    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult Edit(int id)
    {
      var project = db.Projects.Find(id);
      return View("ProjectForm", project);
    }

    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult List()
    {
      if (User.IsInRole("ProjectManager"))
      {
        return View(db.Projects.OrderBy(p => p.CompletedPercentage).ToList());
      }

      return View(projectManagement.GetUsersProject(db.Users.Find(User.Identity.GetUserId())).OrderBy(p => p.CompletedPercentage));

    }
  }
}