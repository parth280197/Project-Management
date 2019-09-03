﻿using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult List(int projectId)
    {
      ViewBag.ProjectId = projectId;
      if (User.IsInRole("ProjectManager"))
        return View(db.Projects.Find(projectId).Tasks.OrderByDescending(t => t.CompletedPercentage).ToList());
      return View(tasksManagement.GetUserTasks(projectId, User.Identity.GetUserId()).OrderByDescending(t => t.Priority));

    }

    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult ListByPriority(int projectId)
    {
      ViewBag.ProjectId = projectId;
      tasksManagement.CheckDeadlines(projectId, User.Identity.GetUserId());
      if (User.IsInRole("ProjectManager"))
        return View("List", db.Projects.Find(projectId).Tasks.OrderByDescending(t => t.Priority).ToList());
      return View("List", tasksManagement.GetUserTasks(projectId, User.Identity.GetUserId()).OrderByDescending(t => t.Priority));

    }

    [Authorize(Roles = "ProjectManager")]
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
      return RedirectToAction("List", new { projectId = userTaskFormViewModel.Task.ProjectId });
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

    [Authorize(Roles = "ProjectManager,Developer")]
    public ActionResult Edit(int taskId)
    {
      if (User.IsInRole("ProjectManager"))
        return View("TasksForm", tasksManagement.LoadViewModel(taskId));
      return View("DevTaskForm", tasksManagement.LoadDevViewModel(taskId));
    }

    [Authorize(Roles = "Developer")]
    public ActionResult DevUpdateTask(DevTaskViewModel devTaskViewModel)
    {
      tasksManagement.DevUpdateTask(devTaskViewModel, User.Identity.GetUserId());
      return RedirectToAction("List", new { id = devTaskViewModel.ProjectId });
    }
  }
}