using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;

using TaskilyWeb.Models;

namespace TaskilyWeb.DAL
{
    public class AdminSecurity
    {
        private static ApplicationUserManager _userManager;
        public static ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public static int GetOrganisationID()
        {
            var user = UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());
            return user.OrganisationID.Value;
        }

        public static Organisation GetOrganisation()
        {
            var user = UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());
            Organisation org = null;
            using (TasklyDbContext db = new TasklyDbContext())
            {
                org = db.Organisations.Find(user.OrganisationID.Value);
            }

            return org;
        }

        /// <summary>
        ///  Security checking...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsValidOrganisation(int id)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return false; 

            if (AdminSecurity.IsInRole(HttpContext.Current.User.Identity.GetUserId(), "Admin"))
                return true; 

            var user = UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());
            return (user.OrganisationID == id);
        }

        public static bool IsValidOrganisation(Organisation organisaion)
        {
            return IsValidOrganisation(organisaion.ID);
        }

        public static bool IsInRole(string user, string role)
        {
            using (TasklyDbContext db = new TasklyDbContext())
            {
                using (var store = new UserStore<TasklyUser>(db))
                {
                    using (var manager = new UserManager<TasklyUser>(store))
                    {
                        return manager.IsInRole(user, role);
                    }
                }
            }
        }
    }
}