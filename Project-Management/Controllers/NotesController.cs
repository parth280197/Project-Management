using Project_Management.Helpers;
using Project_Management.Models;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
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
      tasksManagement = new TasksManagement();
    }
    public ActionResult List(int id)
    {
      ViewBag.Task = tasksManagement.GetUserTask(id);
      var notes = noteManagement.GetTaskNotes(id);
      return View(notes);
    }
  }
}