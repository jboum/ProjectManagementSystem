using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagementSystem.Models;
using System.Security.Claims;

namespace ProjectManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private dbProjectMSEntities projectDb = new dbProjectMSEntities();

        [Authorize]
        public ActionResult Index()
        {
            ClaimsIdentity identity = (ClaimsIdentity) User.Identity;
            Claim userIdClaim = identity.FindFirst("UserID");
            int userId = Convert.ToInt32(userIdClaim.Value);

            var projects = from p in projectDb.Projects where p.UserID == userId select p;
            return View(projects.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}