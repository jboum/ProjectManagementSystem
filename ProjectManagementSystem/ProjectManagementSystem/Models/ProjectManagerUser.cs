using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ProjectManagementSystem.Models {
    public class ProjectManagerUser : IUser<int>{

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ProjectManagerUser, int> manager) {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("UserId", Convert.ToString(this.Id)));
            return userIdentity;
        }
    }
}