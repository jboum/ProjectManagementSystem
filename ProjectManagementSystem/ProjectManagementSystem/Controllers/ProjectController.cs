using System.Linq;
using System.Web.Mvc;
using ProjectManagementSystem.Models;
using System.Security.Claims;
using System;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private dbProjectMSEntities projectDb = new dbProjectMSEntities();

        // GET: Project
        public ActionResult Index(int id)
        {
            var project = from p in projectDb.Projects where p.ProjectID == id select p;

            ViewBag.ProjectId = id;
            return View(project.Single());
        }

        [HttpGet]
        public ActionResult Details(int id) {
            var projectResult = from p in projectDb.Projects where p.ProjectID == id select p;

            ViewBag.ProjectId = id;
            return View(projectResult.Single());
        }

        [HttpPost]
        public ActionResult Details(int id, string projectName, string projectDescription) {
            var projectResult = from p in projectDb.Projects where p.ProjectID == id select p;
            Project project = projectResult.Single();
            project.ProjectName = projectName;
            project.Description = projectDescription;
            projectDb.SaveChanges();

            return RedirectToAction("Index", "Project", new { id = id });
        }

        public ActionResult Risks(int id) {
            var riskResults = from r in projectDb.Risks where r.ProjectID == id select r;

            ViewBag.ProjectId = id;
            return View(riskResults.ToList());
        }

        [HttpPost]
        public ActionResult AddRisk(int id, string riskName, string riskDescription, string riskLevel) {
            Risk risk = new Risk();
            risk.RiskName = riskName;
            risk.RiskDescription = riskDescription;
            risk.RiskLevel = riskLevel;
            risk.ProjectID = id;
            projectDb.Risks.Add(risk);
            projectDb.SaveChanges();

            return RedirectToAction("Risks", "Project", new { id = id });
        }

        [HttpPost]
        public ActionResult DeleteRisk(int id, int deleteRisk) {
            var riskResult = from r in projectDb.Risks where r.RiskID == deleteRisk select r;
            projectDb.Risks.Remove(riskResult.Single());
            projectDb.SaveChanges();

            return RedirectToAction("Risks", "Project", new { id = id });
        }

        public ActionResult Team(int id) {
            var teamResults = from m in projectDb.Members where m.ProjectID == id select m;

            ViewBag.ProjectId = id;
            return View(teamResults.ToList());
        }

        [HttpPost]
        public ActionResult AddMember(int id, string memberName, string memberEmail, string memberPhone) {
            Member member = new Member();
            member.ProjectID = id;
            member.MemberName = memberName;
            member.Email = memberEmail;
            member.Phone_ = memberPhone;
            projectDb.Members.Add(member);
            projectDb.SaveChanges();

            return RedirectToAction("Team", "Project", new { id = id });
        }

        [HttpPost]
        public ActionResult DeleteMember(int id, int memberDeleted) {
            var memberResult = from m in projectDb.Members where m.MemberID == memberDeleted select m;
            projectDb.Members.Remove(memberResult.Single());
            projectDb.SaveChanges();

            return RedirectToAction("Team", "Project", new { id = id });
        }

        [HttpPost]
        public ActionResult Create(string ProjectName) {
            int userId = GetUserId();
            
            Project project = new Project();
            project.ProjectName = ProjectName;
            project.UserID = userId;
            projectDb.Projects.Add(project);
            projectDb.SaveChanges();

            return RedirectToAction("Index", "Project", new { id = project.ProjectID });
        }

        private int GetUserId() {
            ClaimsIdentity identity = (ClaimsIdentity) User.Identity;
            string userId = identity.FindFirst("UserId").Value;
            return Convert.ToInt32(userId);
        }
    }
}