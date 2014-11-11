using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class RequirementsController : Controller
    {
        private dbProjectMSEntities projectDb = new dbProjectMSEntities();

        // GET: Requirements
        public ActionResult Index(int id)
        {
            var resultRequirements = from req in projectDb.Requirements where req.ProjectID == id select req;
            ViewBag.ProjectId = id;
            return View(resultRequirements.ToList());
        }

        [HttpGet]
        public ActionResult Create(int id) {
            ViewBag.ProjectId = id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(int id, string requirementNumber, string requirementName, string requirementDescription, string requirementType) {
            Requirement req = new Requirement();
            req.ProjectID = id;
            req.RequirementType = requirementType;
            req.Description = requirementDescription;
            projectDb.Requirements.Add(req);
            projectDb.SaveChanges();

            ViewBag.ProjectId = id;
            return RedirectToAction("Index", new { id = id });
        }
    }
}