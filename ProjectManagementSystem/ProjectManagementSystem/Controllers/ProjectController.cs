using System.Linq;
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
            ViewBag.ProjectId = id;
            return View(project.Single());
        }

        public ActionResult Details() {
            return View();
        }

        public ActionResult Risks() {
            return View();
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
            Project project = new Project();
            project.ProjectName = ProjectName;
            projectDb.Projects.Add(project);
            projectDb.SaveChanges();

            return RedirectToAction("Index", "Project", new { id = project.ProjectID });
        }
    }
}