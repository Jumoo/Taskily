using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TaskilyWeb.Models;
using TaskilyWeb.DAL;

namespace TaskilyWeb.Controllers
{
    public class ResultsController : Controller
    {
        private TasklyDbContext db = new TasklyDbContext();

        // GET: Results
        // Public Results for a survey
        public ActionResult Index(string id)
        {
            var survey = db.Surveys.Where(x => x.UID == id).SingleOrDefault();

            if (survey == null || !survey.PublicResults)
            {
                return HttpNotFound();
            }

            ViewBag.SurveyTitle = survey.WelcomeTitle;
            ViewBag.SurveyID = survey.ID;
            ViewBag.Tasks = survey.Tasks.Count();
            return View();
        }
        
        public ActionResult Weight(string id)
        {
            var survey = db.Surveys.Where(x => x.UID == id).SingleOrDefault();

            if (survey == null || !survey.PublicResults)
            {
                return HttpNotFound();
            }

            ViewBag.SurveyTitle = survey.WelcomeTitle;
            ViewBag.SurveyID = survey.ID;
            ViewBag.Tasks = survey.Tasks.Count();
            return View();
        }

        public ActionResult Count(string id)
        {
            var survey = db.Surveys.Where(x => x.UID == id).SingleOrDefault();

            if (survey == null || !survey.PublicResults)
            {
                return HttpNotFound();
            }

            ViewBag.SurveyTitle = survey.WelcomeTitle;
            ViewBag.SurveyID = survey.ID;
            ViewBag.Tasks = survey.Tasks.Count();
            ViewBag.Responses = survey.Responses.Count();
            return View();
        }

    }
}