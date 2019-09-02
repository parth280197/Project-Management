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
    public List<Notes> GetTaskNotes(int taskId)
    {
      var notes = db.Tasks.Find(taskId).Notes.ToList();
      return notes;
    }
  }
}