using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class TimeController : Controller
    {
        // GET: Time
        public ActionResult Index(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TimeTotals(int id)
        {
            return View();
        }
    }
}