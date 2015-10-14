using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.Text.RegularExpressions;

using TaskilyWeb.Models;
using TaskilyWeb.DAL;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace TaskilyWeb.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private TasklyDbContext db = new TasklyDbContext();

        public ActionResult Index()
        {
            return View();
        }

        //
        // Survey Wizard
        //

        #region Step 1: Create

        public ActionResult Create()
        {
#if HOSTED_INSTALL
            int orgId = AdminSecurity.GetOrganisationID();         
            var org = db.Organisations.Find(orgId);
            if ( org.AccountType == OrgAccountType.free && org.Surveys.Count > 2 )
            {
                // return new HttpStatusCodeResult(HttpStatusCode.PaymentRequired);
                return RedirectToAction("Index", "Upgrade");
            }
#endif 

            var survey = new Survey();
            survey.OrganisationID = AdminSecurity.GetOrganisationID();
            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Name, OrganisationID")]Survey survey)
        {
            if ( ModelState.IsValid)
            {
                survey.UID = TaskilyHelper.UniqueCode();
                db.Surveys.Add(survey);
                db.SaveChanges();
                return RedirectToAction("AddTasks", new { id = survey.ID });
            }

            return View();
        }

        #endregion 

        #region Step 2: Add Tasks

        public ActionResult AddTasks(int id)
        {
            var survey = db.Surveys.Find(id);

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTasks([Bind(Include="ID, OrganisationID, TaskCount")]Survey survey, string Tasks )
        {

            if (ModelState.IsValid)
            {
                string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

                string[] taskList = Tasks.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string task in taskList)
                {

                    // input.Substring(1, input.Length - 2) removes the first and last " from the string
                    string[] tasksplit = Regex.Split(task, pattern);

                    var dbTask = new SurveyTask
                    {
                        Name = tasksplit[0].Trim(new char[]{' ', '"'}),
                        SurveyID = survey.ID,
                        Description = "",
                        Active = true
                    };

                    if ( tasksplit.Length > 1)
                    {
                        dbTask.Description = tasksplit[1].Trim();
                    }

                    db.Tasks.Add(dbTask);
                }

                db.Surveys.Attach(survey);
                db.Entry(survey).Property(x => x.TaskCount).IsModified = true;
                db.SaveChanges();

                return RedirectToAction("Questions", new { id = survey.ID });
            }

            return View(survey);
        }
        #endregion

        #region Step 3: Questions

        public ActionResult Questions(int id)
        {
            var survey = db.Surveys.Find(id);
            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            ViewBag.ID = survey.ID;
            ViewBag.Questions = db.Questions.Where(q => q.SurveyID == id).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Questions(Question question, int ID)
        {
            if (ModelState.IsValid)
            {
                question.SurveyID = ID;
                db.Questions.Add(question);
                db.SaveChanges();

                ModelState.Clear();

                ViewBag.Questions = db.Questions.Where(q => q.SurveyID == question.SurveyID).ToList();
            }

            ViewBag.ID = ID;
            return View();
        }

        public ActionResult DeleteQuestion(int id)
        {
            var question = db.Questions.Find(id);

            if ( AdminSecurity.IsValidOrganisation(question.Survey.OrganisationID))
            {
                db.Questions.Remove(question);
                db.SaveChanges();
            }

            return RedirectToAction("Questions", new { @id = question.SurveyID });
        }

        #endregion

        #region Step 4: Customize

        [Authorize]
        public ActionResult Customize(int id)
        {
            var survey = db.Surveys.Find(id);

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            
            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Customize(
            [Bind(Include= "ID, OrganisationID, WelcomeMessage, WelcomeTitle, CompleteMessage, CompleteTitle, EndUrl")]Survey survey)
        {
            if ( ModelState.IsValid )
            {
                if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Invalid Org " + survey.OrganisationID);

                db.Surveys.Attach(survey);

                db.Entry(survey).Property(x => x.WelcomeMessage).IsModified = true;
                db.Entry(survey).Property(x => x.WelcomeTitle).IsModified = true;
                db.Entry(survey).Property(x => x.CompleteMessage).IsModified = true;
                db.Entry(survey).Property(x => x.CompleteTitle).IsModified = true;
                db.Entry(survey).Property(x => x.EndUrl).IsModified = true;
                db.SaveChanges();

                return RedirectToAction("Finish", new { @id = survey.ID });
            }

            return View(survey);
        }

        #endregion

        #region Step 5: Finish
        [Authorize]
        public ActionResult Finish(int id)
        {
            var survey = db.Surveys.Find(id);
            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finish(Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Surveys.Attach(survey);
                db.Entry(survey).Property(x => x.Active).IsModified = true;
                db.SaveChanges();

                return RedirectToAction("Index", "Admin");
            }

            return View(survey);
        }
        
        #endregion

        //
        // TODO: Moving the Edit stuff into another controller for sanity. 
        //
        #region Delete and Clean

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Survey survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(survey);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

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
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");

        }

        public ActionResult Clean(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Survey survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Clean(int id)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

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

            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }


        #endregion

        public ActionResult Popup(int id)
        {
            var survey = db.Surveys.Find(id);
            return View(survey);
        }

        //
        // View and Edit Existing 
        //
        #region View and Edit

        public ActionResult Details(int id)
        {
            var survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            ViewBag.Limit = "unlimited";
            ViewBag.SurveyFull = false;

            ViewBag.IsAdmin = AdminSecurity.IsInRole(User.Identity.GetUserId(), "Admin");

#if HOSTED_INSTALL
            int orgId = AdminSecurity.GetOrganisationID();
            var org = db.Organisations.Find(orgId);

            if ( org.AccountType == OrgAccountType.free )
            {
                ViewBag.Limit = ((int)HostedConfig.FreeResponses).ToString() ;
                if ( survey.Responses.Count >= (int)HostedConfig.FreeResponses )
                {
                    ViewBag.SurveyFull = true;
                }
            }
#endif

            return View(survey);

        }

        public ActionResult Edit(int id)
        {
            var survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            ViewBag.FullAccount = survey.organisation.AccountType != OrgAccountType.free;

            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBasic([Bind(Include = "ID,OrganisationID,Name,Active,TaskCount,PublicResults")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Surveys.Attach(survey);
                if (AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                {
                    db.Entry(survey).Property(x => x.Name).IsModified = true;
                    db.Entry(survey).Property(x => x.Active).IsModified = true;
                    db.Entry(survey).Property(x => x.PublicResults).IsModified = true;
                    
                    if( survey.TaskCount > 0 ) 
                        db.Entry(survey).Property(x => x.TaskCount).IsModified = true;

                    db.SaveChanges();
                }
            }

            return RedirectToAction("Edit", new { @id = survey.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomEdit([Bind(Include = "ID,OrganisationID, WelcomeMessage, WelcomeTitle, CompleteMessage, CompleteTitle, EndUrl")] Survey survey)
        {
            if ( ModelState.IsValid)
            {
                db.Surveys.Attach(survey);

                if (AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                {
                    db.Entry(survey).Property(x => x.WelcomeTitle).IsModified = true;
                    db.Entry(survey).Property(x => x.WelcomeMessage).IsModified = true;
                    db.Entry(survey).Property(x => x.CompleteTitle).IsModified = true;
                    db.Entry(survey).Property(x => x.CompleteMessage).IsModified = true;
                    db.Entry(survey).Property(x => x.EndUrl).IsModified = true;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Edit", new { @id = survey.ID });
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrizeDraw([Bind(Include="ID, OrganisationID, Draw, DrawTitle, DrawMessage, MarketingOptIn, MarketingMessage")] Survey survey )
        { 
        
            if ( ModelState.IsValid)
            {
                db.Surveys.Attach(survey);
                if ( AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                {
                    db.Entry(survey).Property(x => x.Draw).IsModified = true;
                    db.Entry(survey).Property(x => x.DrawTitle).IsModified = true;
                    db.Entry(survey).Property(x => x.DrawMessage).IsModified = true;

                    db.Entry(survey).Property(x => x.MarketingOptIn).IsModified = true;
                    db.Entry(survey).Property(x => x.MarketingMessage).IsModified = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Edit", new { @id = survey.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdvanced([Bind(Include = "ID, OrganisationID, CssFile, HeaderHtml, FooterHtml, TasksHeading, TasksSubHeading, OrderHeading, OrderSubHeading, OrderText, QuestionHeading, QuestionSubHeading, QuestionText")] Survey survey)
        {
            
            if (ModelState.IsValid)
            {
                db.Surveys.Attach(survey);

                if (AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                {

                    db.Entry(survey).Property(x => x.CssFile).IsModified = true;
                    db.Entry(survey).Property(x => x.HeaderHtml).IsModified = true;
                    db.Entry(survey).Property(x => x.FooterHtml).IsModified = true;
                    db.Entry(survey).Property(x => x.TasksHeading).IsModified = true;
                    db.Entry(survey).Property(x => x.TasksSubHeading).IsModified = true;
                    db.Entry(survey).Property(x => x.OrderHeading).IsModified = true;
                    db.Entry(survey).Property(x => x.OrderSubHeading).IsModified = true;
                    db.Entry(survey).Property(x => x.OrderText).IsModified = true;
                    db.Entry(survey).Property(x => x.QuestionHeading).IsModified = true;
                    db.Entry(survey).Property(x => x.QuestionSubHeading).IsModified = true;
                    db.Entry(survey).Property(x => x.QuestionText).IsModified = true;
                    db.SaveChanges();

                }

            }
            return RedirectToAction("Edit", new { @id = survey.ID });
        }
        #endregion
        
        #region Edit Tabs
        /*
        public ActionResult EditQuestions(int id)
        {
            var survey = db.Surveys.Find(id);
            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            ViewBag.Survey = survey;
            ViewBag.Questions = db.Questions.Where(q => q.SurveyID == id).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestions(Question question,int id)
        {
            if (ModelState.IsValid)
            {
                question.SurveyID = id;
                db.Questions.Add(question);
                db.SaveChanges();

                ModelState.Clear();

                ViewBag.Questions = db.Questions.Where(q => q.SurveyID == question.SurveyID).ToList();
            }

            ViewBag.Survey = db.Surveys.Find(id);
            return View();
        }

        public ActionResult EditCustomize(int id)
        {
            return GetSurvey(id);
        }

        public ActionResult EditAdvanced(int id)
        {
            return GetSurvey(id);
        }

        public ActionResult EditTasks(int id)
        {
            return GetSurvey(id);
        }
        */
        #endregion

        private ActionResult GetSurvey(int id)
        {
            var survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(survey);
        }
    }
}