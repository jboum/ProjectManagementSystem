using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers {
    public class UserController : Controller {
        private ProjectUserManager userManager;

        public UserController() { }

        public UserController(ProjectUserManager setUserManager) {
            userManager = setUserManager;
        }

        public ProjectUserManager UserManager {
            get {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ProjectUserManager>();
            }
            private set {
                userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string userName, string password, string returnUrl) {
            ProjectManagerUser user = await UserManager.FindAsync(userName, password);

            if (user != null) {
                IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                authManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                authManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, await user.GenerateUserIdentityAsync(UserManager));

                if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl)) {
                    return RedirectToAction("Index", "Home");
                } else {
                    return Redirect(returnUrl);
                }
            } else {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(string userName, string email, string password, string confirmPassword) {
            ProjectManagerUser user = new ProjectManagerUser();
            user.UserName = userName;
            user.Email = email;

            if (!password.Equals(confirmPassword)) {
                ViewBag.ErrorName = "PasswordMismatch";
                return View();
            }

            IdentityResult result = await UserManager.CreateAsync(user, password);

            if (result.Succeeded) {
                IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                authManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                authManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, await user.GenerateUserIdentityAsync(UserManager));
                return RedirectToAction("Index", "Home");
            } else {
                ViewBag.ErrorName = "RegisterFailed";
                ViewBag.RegisterErrors = new List<string>();

                foreach (string error in result.Errors) {
                    ViewBag.RegisterErrors.Add(error);
                }

                return View();
            }
        }

        [Authorize]
        public ActionResult Logout() {
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login", "User");
        }
    }
}