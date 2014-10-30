using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private dbProjectMSEntities projectDb = new dbProjectMSEntities();

        // GET: Project
        public ActionResult Index(int id)
        {
            var project = from p in projectDb.Projects where p.ProjectID == id select p;
            return View(project.Single());
        }

        public ActionResult Details() {
            return View();
        }

        public ActionResult Risks() {
            return View();
        }

        public ActionResult Team() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string ProjectName) {
            Project project = new Project();
            project.ProjectName = ProjectName;
            projectDb.Projects.Add(project);
            projectDb.SaveChanges();

            return RedirectToAction("Index", "Project", new { id = project.ProjectID });
        }
    }
}