using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TaskilyWeb.DAL;
using TaskilyWeb.Models;

namespace TaskilyWeb.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        TasklyDbContext db = new TasklyDbContext();

        // GET: Data
        public FileContentResult GetData(int id)
        {
            var survey = db.Surveys.Find(id);

            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return null;

            string results = "ID, Date,";

            for (int x = survey.TaskCount; x > 0; x--)
            {
                results += x.ToString() + ",";
            }

            foreach(var q in survey.Questions)
            {
                results += "\"" + q.Text + "\",";
            }

            results += "\n";

            foreach(var response in survey.Responses)
            {
                string line = response.ID.ToString() + "," ;

                if (response.Completed != null)
                {
                    line += response.Completed.Value.ToString("dd-MMM-yyyy HH:mm") + ",";
                }
                
                foreach(var picked in response.Picked.OrderByDescending(x => x.Weight))
                {
                    line += picked.Task.Name + ",";
                }

                foreach(var q in survey.Questions)
                {
                    if ( response.Answers.Any(x => x.QuestionID == q.ID) )
                    {
                        line += "\"" + response.Answers.Where(x => x.QuestionID == q.ID).FirstOrDefault().AnswerText + "\",";
                    }
                    else {
                        line += "\"\","; 
                    }
                }

                results += line.TrimEnd(',') + "\n"; 
            }

            return File(new System.Text.UTF8Encoding().GetBytes(results), "text/csv", survey.Name + "_" + DateTime.Now.ToString("ddMMyy_HHmm") + "_full.csv");

        }

        public FileContentResult GetSummary(int id)
        {
            var survey = db.Surveys.Find(id);
            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return null;

            StatsController stats = new StatsController();
            var summary = stats.GetSummary(id);

            var results = "TaskID, TaskName, Weight, WeightPercent, Count, CountPercent, Importance\n";

            foreach(var task in summary.Tasks.OrderByDescending(x => x.Weight))
            {
                results += task.ID + "," + task.Name + "," + task.Weight + "," + task.WeightPercent + "," + task.Count + "," + task.CountPercent + "," + task.Importance + "\n";
            }

            return File(new System.Text.UTF8Encoding().GetBytes(results), "text/csv", survey.Name + "_" + DateTime.Now.ToString("ddMMyy_HHmm") + "_summary.csv");
        }

        public FileContentResult GetDraw(int id)
        {
            var survey = db.Surveys.Find(id);
            if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                return null;

            var results = "Name,Email,OptIn\n"; 
            if (survey.Draw)
            {
                foreach(var d in survey.Draws)
                {
                    results += d.Name + "," + d.Email + "," + d.contact +"\n";
                }
            }

            return File(new System.Text.UTF8Encoding().GetBytes(results), "text/csv", survey.Name + "_" + DateTime.Now.ToString("ddMMyy_HHMM") + "_prizedraw.csv");
        }
    }
}