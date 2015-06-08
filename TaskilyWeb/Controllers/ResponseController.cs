using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Data;
using System.Data.Entity;

using TaskilyWeb.Models;
using TaskilyWeb.DAL;

namespace TaskilyWeb.Controllers
{
    public class ResponseController : Controller
    {
        private TasklyDbContext db = new TasklyDbContext();

        public ActionResult Welcome(string id)
        {
            if (String.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Survey survey = db.Surveys.Where(s => s.UID == id).SingleOrDefault();
            // Survey survey = db.Surveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            // let authenticated people see a survey that isn't active...
            if (!survey.Active && !User.Identity.IsAuthenticated )
                return new HttpStatusCodeResult(HttpStatusCode.Gone);

            /* hosted - free accounts get 50 responses */
#if HOSTED_INSTALL
            if ( survey.organisation.AccountType == OrgAccountType.free && survey.Responses.Count >= (int)HostedConfig.FreeResponses )
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }
#endif

            return View(survey);
        }

        public ActionResult Tasks(string id)
        {
            if ( String.IsNullOrEmpty(id) )
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Survey survey = db.Surveys
                .Include(s => s.Tasks)
                .Where(s => s.UID == id)
                .Single();

            if (!survey.Active && !User.Identity.IsAuthenticated)
                return new HttpStatusCodeResult(HttpStatusCode.Gone);

            // shuffle and hide inactive tasks.
            survey.Tasks = survey.Tasks.OrderBy(x => Guid.NewGuid()).Where(t => t.Active == true).ToList();
            return View(survey);
        }

        public ActionResult Order(string id, string[] tasks)
        {
            List<SurveyTask> ordered = new List<SurveyTask>();

            foreach(var task in tasks)
            {
                int taskId;
                if (int.TryParse(task, out taskId))
                {
                    var surveyTask = db.Tasks.Find(taskId);
                    if (surveyTask != null)
                        ordered.Add(surveyTask);
                }
            }

            ViewBag.SurveyId = id;

            // advanced customization stuff.
            var survey = db.Surveys.Where(s => s.UID == id).FirstOrDefault(); 
            ViewBag.Css = survey.CssFile ;
            ViewBag.Header = survey.HeaderHtml;
            ViewBag.Footer = survey.FooterHtml;
            ViewBag.Heading = survey.OrderHeading;
            ViewBag.SubHeading = survey.OrderSubHeading;
            ViewBag.Summary = survey.OrderText;

            return View(ordered);
        }

        [HttpPost]
        public ActionResult SaveOrder(string id, string order)
        {
            if ( string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var survey = db.Surveys.Where(s => s.UID == id).Single();
            // var survey = db.Surveys.Find(id);

            if (survey == null)
                return HttpNotFound();

            var response = new Response();
            response.SurveyID = survey.ID;
            db.Responses.Add(response);
            db.SaveChanges();

            var score = survey.TaskCount;
            foreach(string taskid in order.Split(','))
            {
                int pickedId;
                if ( int.TryParse(taskid, out pickedId))
                {
                    var task = new Picked();
                    task.SurveyTaskID = pickedId;
                    task.ResponseID = response.ID;
                    task.Weight = score;
                    db.Picked.Add(task);
                    score--;
                }
            }

            db.SaveChanges();
            return RedirectToAction("Questions", new { @id = response.ID });
        }

        public ActionResult Questions(int id)
        {
            var response = db.Responses.Find(id);
            if (response == null)
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);

            var survey = response.Survey;

            if (survey.Questions.Count() == 0)
            {
                if (!survey.Draw)
                {
                    return RedirectToAction("Complete", new { @id = response.ID });
                }
                else
                {
                    return RedirectToAction("Draw", new { @id = id });
                }
            }
                

            ViewBag.Questions = survey.Questions;

            // advanced customization stuff.
            ViewBag.Css = survey.CssFile;
            ViewBag.Header = survey.HeaderHtml;
            ViewBag.Footer = survey.FooterHtml;
            ViewBag.Heading = survey.QuestionHeading;
            ViewBag.SubHeading = survey.QuestionSubHeading;
            ViewBag.Summary = survey.QuestionText;

            return View(response);
        }

        [HttpPost]
        public ActionResult Questions(int id, FormCollection formCollection)
        {
            var response = db.Responses.Find(id);
            if (response == null)
                return HttpNotFound();

            foreach(var key in formCollection.AllKeys)
            {
                if ( key.StartsWith("qst_"))
                {
                    var val = formCollection[key];
                    var qID = key.Substring(key.IndexOf("_") + 1);

                    int questionID;
                    if (int.TryParse(qID, out questionID))
                    {
                        var answer = new Answer
                        {
                            QuestionID = questionID,
                            AnswerText = val,
                            Response = response
                        };
                        db.Answers.Add(answer);
                    }
                }
            }

            db.SaveChanges();
          
            return RedirectToAction("Draw", new { @id = response.ID });
        }

        public ActionResult Draw(int id)
        {
            var response = db.Responses.Find(id);
            if (response == null)
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            var survey = response.Survey;

            if ( !survey.Draw )
                return RedirectToAction("Complete", new { @id = response.ID });

            // advanced customization stuff.
            ViewBag.Css = survey.CssFile;
            ViewBag.Header = survey.HeaderHtml;
            ViewBag.Footer = survey.FooterHtml;
            ViewBag.Heading = survey.DrawTitle;
            ViewBag.Summary = survey.DrawMessage;
            ViewBag.Marketing = survey.MarketingOptIn;
            ViewBag.MarketingMsg = survey.MarketingMessage;

            return View(response);
        }

        [HttpPost]
        public ActionResult Draw(int id, string name, string email, bool? contact)
        {
            var response = db.Responses.Find(id);
            if (response == null)
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);


            var survey = response.Survey;
            
            var drawDetails = new PrizeDrawNames();
            drawDetails.Name = name;
            drawDetails.Email = email;
            if ( contact != null )
                drawDetails.contact = contact.Value;

            survey.Draws.Add(drawDetails);
            db.SaveChanges();

            return RedirectToAction("Complete", new { @id = response.ID });
        }

        public ActionResult Complete(int id)
        {
            var response = db.Responses.Find(id);
            if (response == null)
                return HttpNotFound();

            response.Completed = DateTime.Now;
            db.Entry(response).Property(x => x.Completed).IsModified = true;
            db.SaveChanges();

            return View(response.Survey);
        }
    }
}