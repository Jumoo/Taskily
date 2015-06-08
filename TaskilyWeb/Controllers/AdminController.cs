using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskilyWeb.DAL;
using TaskilyWeb.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace TaskilyWeb.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private TasklyDbContext db = new TasklyDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            var orgId = AdminSecurity.GetOrganisationID();
            var orgs = db.Organisations.ToList().Where(o => o.ID == orgId);

            ViewBag.SurveyLimit = false;

            bool isAdmin = AdminSecurity.IsInRole(User.Identity.GetUserId(), "Admin");
            ViewBag.IsAdmin = isAdmin;

            if (isAdmin)
                orgs = db.Organisations.ToList();

#if HOSTED_INSTALL

            if(!isAdmin)
            {
                var firstOrg = orgs.FirstOrDefault();
                switch(firstOrg.AccountType)
                {
                    case OrgAccountType.free:
                        if (firstOrg.Surveys.Count >= 3)
                        {
                            ViewBag.SurveyLimit = true;
                        }
                        break;
                    case OrgAccountType.standard:
                        if ( firstOrg.Surveys.Count >= 5)
                        {
                            ViewBag.SurveyLimit = true;
                        }
                        break;

                    default:
                        break;
                }
            }

#endif


            return View(orgs.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!AdminSecurity.IsValidOrganisation(id.Value))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            Organisation organisation = db.Organisations.Find(id);
            if (organisation == null)
            {
                return HttpNotFound();
            }
            return View(organisation);
        }

        // GET: Admin/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Name,Website,Address,AccountType,TrialDate")] Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                db.Organisations.Add(organisation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organisation);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!AdminSecurity.IsValidOrganisation(id.Value))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            bool isAdmin = AdminSecurity.IsInRole(User.Identity.GetUserId(), "Admin");
            ViewBag.IsAdmin = isAdmin;

            Organisation organisation = db.Organisations.Find(id);
            if (organisation == null)
            {
                return HttpNotFound();
            }
            return View(organisation);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Website,Address,AccountType,TrialDate")] Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organisation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organisation);
        }

        // GET: Admin/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (!AdminSecurity.IsInRole(User.Identity.GetUserId(), "Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "No Delete...");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organisation organisation = db.Organisations.Find(id);
            if (organisation == null)
            {
                return HttpNotFound();
            }
            return View(organisation);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSecurity.IsInRole(User.Identity.GetUserId(), "Admin"))
            {
                // Need to remove surveys
                var surveys = db.Surveys.Where(x => x.OrganisationID == id).ToList() ;

                foreach (var survey in surveys)
                {
                    // manually delete the response stuff.
                    var responses = db.Responses.Where(r => r.SurveyID == survey.ID).ToList();
                    foreach (var response in responses)
                    {
                        foreach (var pick in db.Picked.Where(p => p.ResponseID == response.ID))
                        {
                            db.Picked.Remove(pick);
                        }

                        db.Responses.Remove(response);

                    }
                    db.Surveys.Remove(survey);
                }

                var users = db.Users.Where(x => x.OrganisationID == id).ToList();

                foreach(var user in users)
                {
                    user.OrganisationID = null;
                    db.Users.Remove(user);

                    if ( AdminSecurity.IsInRole( user.Id, "Admin"))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Can't delete the admins org");
                    }

                }

                Organisation organisation = db.Organisations.Find(id);
                db.Organisations.Remove(organisation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Not allowed");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
