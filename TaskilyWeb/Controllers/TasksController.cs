using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Data;
using System.Data.Entity;

using TaskilyWeb.Models;
using TaskilyWeb.DAL;
using System.Web.Http.Description;

namespace TaskilyWeb.Controllers
{
    public class TasksController : ApiController
    {
        private TasklyDbContext db = new TasklyDbContext();

        [ResponseType(typeof(SurveyTask))]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult Delete(int id)
        {

            SurveyTask task = db.Tasks.Find(id);
            if (task == null)
                return NotFound();

            var survey = task.survey;

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return BadRequest("Not allowed");

            if ( survey.Responses.Count > 0 )
            {
                task.Active = false;
                db.Entry(task).Property(p => p.Active).IsModified = true;
            }
            else
            {
                db.Tasks.Remove(task);
            }

            db.SaveChanges();

            return Ok(task);
        }

        [ResponseType(typeof(SurveyTask))]
        [HttpPut]
        [Authorize]
        public IHttpActionResult Activate(int id)
        {
            SurveyTask task = db.Tasks.Find(id);
            if (task == null)
                return NotFound();

            var survey = task.survey;

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return BadRequest("Not allowed");

            if (task.Active == false)
            {
                task.Active = true;
                db.Entry(task).Property(p => p.Active).IsModified = true;
            }

            db.SaveChanges();
            return Ok(task);
        }


        [ResponseType(typeof(SurveyTask))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult Update(int id, SurveyTask task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var survey = db.Surveys.Find(id);
            if (survey == null)
                return BadRequest("No survey");

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return BadRequest("Not allowed");

            db.Tasks.Attach(task);
            task.survey = survey; 
            db.Entry(task).Property(p => p.Name).IsModified = true;
            db.Entry(task).Property(p => p.Description).IsModified = true;
            db.SaveChanges();

            return Ok(task);
        }

        [ResponseType(typeof(Survey))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult Add(int id, SurveyTask task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id > 0)
            {
                var survey = db.Surveys.Find(id);
                if (survey == null)
                    return BadRequest("No Survey");

                if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                    return BadRequest("Not Allowed to Add");

                task.survey = survey;
                db.Tasks.Add(task);
                db.SaveChanges();

                return CreatedAtRoute("DefaultAPI", new { id = task.ID }, task);
            }

            return BadRequest("SurveyID was blank");
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<SurveyTask> All(int id)
        {
            List<SurveyTask> tasks = new List<SurveyTask>();

            var survey = db.Surveys.Where(s => s.ID == id).FirstOrDefault();

            if ( survey == null)
                return null;

            if ( !AdminSecurity.IsValidOrganisation(survey.OrganisationID)) 
                return null;
        
            return db.Tasks.Where(t => t.SurveyID == id).ToList();
        }

    }
}
