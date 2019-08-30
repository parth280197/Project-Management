﻿using Microsoft.AspNet.Identity;
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
      return View("ProjectForm");
    }
    [HttpPost]
    public ActionResult Create(Project project)
    {
      if (ModelState.IsValid)
      {
        project.CompletedPercentage = 0;
        project.CreatedBy = db.Users.Find(User.Identity.GetUserId());
        projectManagement.CreateProject(project);
      }
      else
      {
        return HttpNotFound();
      }
      return View("List");
    }
    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult List()
    {
      return View(db.Projects.ToList());
    }
  }
}