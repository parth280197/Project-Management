﻿using Project_Management.Helpers;
using Project_Management.Models;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
  [Authorize(Roles = "ProjectManager,Developer")]
  public class NotesController : Controller
  {
    // GET: Notes
    private NotesManagement noteManagement;
    private TasksManagement tasksManagement;
    private ApplicationDbContext db;
    public NotesController()
    {
      db = new ApplicationDbContext();
      noteManagement = new NotesManagement();
      tasksManagement = new TasksManagement(db);
    }
    public ActionResult List(int id)
    {
      ViewBag.Task = tasksManagement.GetUserTask(id);
      var notes = noteManagement.GetTaskNotes(id);
      return View(notes);
    }
  }
}