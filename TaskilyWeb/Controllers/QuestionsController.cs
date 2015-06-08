using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskilyWeb.DAL;
using TaskilyWeb.Models;

namespace TaskilyWeb.Controllers
{
    [Authorize]
    public class QuestionsController : ApiController
    {
        private TasklyDbContext db = new TasklyDbContext();

        // GET: api/Questions/All/5
        [HttpGet]
        public IEnumerable<Question> All(int id)
        {
            var survey = db.Surveys.Find(id);

            if (survey == null)
                return null;

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return null;

            return db.Questions.Where(q => q.SurveyID == id).ToList();
        }

        // GET: api/Questions/Get/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult Get(int id)
        {
            Question question = db.Questions.Find(id);

            if (question == null)
                return NotFound();

            if (!AdminSecurity.IsValidOrganisation(question.Survey.OrganisationID))
                return BadRequest("Not allowed");

            return Ok(question);
        }

        // POST: api/Questions/Save/
        [HttpPost]
        [ResponseType(typeof(Question))]
        public IHttpActionResult Add(int id, Question question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id > 0)
            {
                var survey = db.Surveys.Find(id);
                if (survey == null)
                    return BadRequest("No Survey");

                if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                    return BadRequest("Not Allowed to add");

                question.SurveyID = id;
                if (question.ID == 0)
                {
                    db.Questions.Add(question);
                }
                else
                {
                    db.Questions.Attach(question);
                    db.Entry(question).State = EntityState.Modified;
                }
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = question.ID }, question);
            }
            return BadRequest("SurveyID was blank");
        }

        // DELETE: api/Questions/Delete/5
        [HttpDelete]
        [ResponseType(typeof(Question))]
        public IHttpActionResult Delete(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            if (!AdminSecurity.IsValidOrganisation(question.Survey.OrganisationID))
                return BadRequest("Not allowed");
            
            db.Questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.ID == id) > 0;
        }
    }
}