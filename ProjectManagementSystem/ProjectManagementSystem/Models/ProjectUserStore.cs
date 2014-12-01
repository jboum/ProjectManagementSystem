using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Models {
    public class ProjectUserStore : IUserEmailStore<ProjectManagerUser, int>, IUserPasswordStore<ProjectManagerUser, int> {

        private dbProjectMSEntities projectDb = new dbProjectMSEntities();
        private Dictionary<string, string> temporaryHashes = new Dictionary<string, string>();
        private Dictionary<string, string> temporaryEmails = new Dictionary<string, string>();

        public Task CreateAsync(ProjectManagerUser user) {
            User dbUser = new User();
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;

            if (temporaryHashes.ContainsKey(dbUser.UserName)) {
                dbUser.Password = temporaryHashes[dbUser.UserName];
                temporaryHashes.Remove(dbUser.UserName);
            }

            if (temporaryEmails.ContainsKey(dbUser.UserName)) {
                dbUser.Email = temporaryEmails[dbUser.UserName];
                temporaryEmails.Remove(dbUser.UserName);
            }

            projectDb.Users.Add(dbUser);
            projectDb.SaveChanges();
            user.Id = dbUser.UserID;
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(ProjectManagerUser user) {
            var userResult = from u in projectDb.Users where u.UserID == user.Id select u;
            projectDb.Users.Remove(userResult.Single());
            return projectDb.SaveChangesAsync();
        }

        public Task<ProjectManagerUser> FindByIdAsync(int userId) {
            var userResult = from u in projectDb.Users where u.UserID == userId select u;

            User dbUser = userResult.Single();
            ProjectManagerUser user = new ProjectManagerUser();
            user.Id = userId;
            user.UserName = dbUser.UserName;
            user.Email = dbUser.Email;
            return Task.FromResult<ProjectManagerUser>(user);
        }

        public Task<ProjectManagerUser> FindByNameAsync(string userName) {
            var userResult = from u in projectDb.Users where u.UserName.Equals(userName) select u;

            try {
                User dbUser = userResult.Single();
                ProjectManagerUser user = new ProjectManagerUser();
                user.Id = dbUser.UserID;
                user.UserName = dbUser.UserName;
                user.Email = dbUser.Email;
                return Task.FromResult<ProjectManagerUser>(user);
            }
            catch (InvalidOperationException) {
                return Task.FromResult<ProjectManagerUser>(null);
            }
        }

        public Task UpdateAsync(ProjectManagerUser user) {
            var userResult = from u in projectDb.Users where u.UserID == user.Id select u;
            User dbUser = userResult.Single();
            dbUser.Email = user.Email;
            return projectDb.SaveChangesAsync();
        }

        public void Dispose() {
        }

        public Task<string> GetPasswordHashAsync(ProjectManagerUser user) {
            var userResult = from u in projectDb.Users where u.UserID == user.Id select u;
            User dbUser = userResult.Single();
            return Task.FromResult<string>(dbUser.Password);
        }

        public Task<bool> HasPasswordAsync(ProjectManagerUser user) {
            return Task.FromResult<bool>(true);
        }

        public Task SetPasswordHashAsync(ProjectManagerUser user, string passwordHash) {
            if (user.Id > 0) {
                var userResult = from u in projectDb.Users where u.UserID == user.Id select u;
                User dbUser = userResult.Single();
                dbUser.Password = passwordHash;
                return Task.FromResult(projectDb.SaveChangesAsync());
            } else {
                temporaryHashes.Add(user.UserName, passwordHash);
                return Task.FromResult<object>(null);
            }
        }

        private bool HasPassword(ProjectManagerUser user) {
            return true;
        }

        public Task<ProjectManagerUser> FindByEmailAsync(string email) {
            var userResult = from u in projectDb.Users where u.Email.Equals(email) select u;

            try {
                User dbUser = userResult.Single();
                ProjectManagerUser user = new ProjectManagerUser();
                user.Id = dbUser.UserID;
                user.UserName = dbUser.UserName;
                user.Email = dbUser.Email;
                return Task.FromResult(user);
            }
            catch (InvalidOperationException) {
                return Task.FromResult<ProjectManagerUser>(null);
            }
        }

        public Task<string> GetEmailAsync(ProjectManagerUser user) {
            if (user.Id < 1) {
                return Task.FromResult(temporaryEmails[user.UserName]);
            } else {
                var userResult = from u in projectDb.Users where u.UserID == user.Id select u;
                User dbUser = userResult.Single();
                return Task.FromResult(dbUser.Email);
            }
        }

        public Task<bool> GetEmailConfirmedAsync(ProjectManagerUser user) {
            return Task.FromResult(true);
        }

        public Task SetEmailAsync(ProjectManagerUser user, string email) {
            var userResult = from u in projectDb.Users where u.UserID == user.Id select u;

            if (user.Id > 0) {
                User dbUser = userResult.Single();
                dbUser.Email = email;
                return projectDb.SaveChangesAsync();
            } else {
                temporaryEmails.Add(user.UserName, email);
                return Task.FromResult<object>(null);
            }

        }

        public Task SetEmailConfirmedAsync(ProjectManagerUser user, bool confirmed) {
            return Task.FromResult<object>(null);
        }
    }
}