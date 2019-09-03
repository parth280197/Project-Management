using Microsoft.AspNet.Identity;
using Project_Management.Helpers;
using Project_Management.Models;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
  public class NotificationsController : Controller
  {
    private ApplicationDbContext db;
    private NotificationManagement notificationManagement;
    public NotificationsController()
    {
      db = new ApplicationDbContext();
      notificationManagement = new NotificationManagement(db);
    }
    // GET: Notoifications
    public ActionResult List()
    {
      var notifications = notificationManagement.GetNotifications(db.Users.Find(User.Identity.GetUserId()));
      return View(notifications);
    }
  }
}