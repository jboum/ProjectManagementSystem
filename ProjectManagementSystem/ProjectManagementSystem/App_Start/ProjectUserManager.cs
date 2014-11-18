using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;

namespace ProjectManagementSystem {
    public class ProjectUserManager : UserManager<ProjectManagerUser, int> {
        public ProjectUserManager() : base(new ProjectUserStore()){
        }

        public static ProjectUserManager Create(IdentityFactoryOptions<ProjectUserManager> options, IOwinContext context) 
        {
            var manager = new ProjectUserManager();
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ProjectManagerUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ProjectManagerUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}