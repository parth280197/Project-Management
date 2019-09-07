using Project_Management.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project_Management.Helpers
{
  public class NotesManagement
  {
    private ApplicationDbContext db;
    public NotesManagement()
    {
      db = new ApplicationDbContext();
    }

    /// <summary>
    /// Gives all notes associated for specific task.
    /// </summary>
    /// <param name="taskId">To get task and all notes associated with that task.</param>
    /// <returns>All notes made for specific task.</returns>
    public List<Notes> GetTaskNotes(int taskId)
    {
      var notes = db.Tasks.Find(taskId).Notes.ToList();
      return notes;
    }
  }
}