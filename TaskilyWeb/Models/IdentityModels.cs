using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using TaskilyWeb.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace TaskilyWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class TasklyUser : IdentityUser
    {
        public string UserDisplayName { get; set; }

        public int? OrganisationID { get; set; }
        public virtual Organisation Organisation { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<TasklyUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


}