using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class TimeController : Controller
    {
        private dbProjectMSEntities projectDb = new dbProjectMSEntities();

        // GET: Time
        public ActionResult Index(int id)
        {
            var reqsResult = from req in projectDb.Requirements where req.ProjectID == id select req;
            var phasesResult = from phase in projectDb.Phases select phase;
            ViewBag.ProjectId = id;
            return View(new TimeCreateModel {Requirements = reqsResult.ToList(), Phases = phasesResult.ToList()});
        }

        [HttpPost]
        public ActionResult LogTime(int id, int phaseId, int requirementId, float hours) {
            Hour time = new Hour() {
                PhaseID = phaseId,
                RequirementID = requirementId,
                Hours = hours
            };
            projectDb.Hours.Add(time);
            projectDb.SaveChanges();

            return RedirectToAction("Index", new { id = id });
        }

        [HttpGet]
        public ActionResult TimeTotals(int id)
        {
            var timesResult = from t in projectDb.Hours where t.Requirement.ProjectID == id select t;
            List<Hour> times = timesResult.ToList();
            
            ViewBag.ProjectId = id;
            return View(times);
        }
    }
}