using Project_Management.Models;

namespace Project_Management.Helpers
{
  public class TasksManagement
  {
    private ApplicationDbContext db;
    public TasksManagement()
    {
      db = new ApplicationDbContext();
    }
    public bool CreateTask(UserTask userTask, string[] developers)
    {
      if (userTask != null)
      {
        db.Tasks.Add(userTask);
        db.SaveChanges();
        foreach (string developerId in developers)
        {
          var user = db.Users.Find(developerId);
          userTask.Users.Add(user);
          db.SaveChanges();
        }
        return true;
      }
      return false;
    }
  }
}