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

namespace TaskilyWeb.Controllers
{
    public class StatsController : ApiController
    {
        // private TasklyDbContext db = new TasklyDbContext();

        //
        // GET: /API/Stats/GetSummary/id
        //
        public SurveySummary GetSummary(int id)
        {
            SurveySummary data = new SurveySummary();
            int totalCount = 0;
            int totalWeight = 0;
            int responseCount = 0;
            int maxWeight = 0;

            using (TasklyDbContext db = new TasklyDbContext())
            {
                var survey = db.Surveys.Find(id);
                maxWeight = survey.TaskCount;
                
                if (!AdminSecurity.IsValidOrganisation(survey.OrganisationID))
                {
                    // public results (for non-
                    if ( survey.organisation.AccountType == OrgAccountType.free || !survey.PublicResults )
                        return null;
                }
                    

                var responses = db.Responses
                    .Include(r => r.Picked)
                    .Where(r => r.SurveyID == id).ToList();

                foreach (var response in responses)
                {
                    foreach (var task in response.Picked)
                    {
                        if (!data.Tasks.Any(x => x.ID == task.SurveyTaskID))
                        {
                            data.Tasks.Add(new ResponseData
                                {
                                    ID = task.SurveyTaskID,
                                    Name = task.Task.Name,
                                    Count = 1,
                                    Weight = task.Weight
                                });
                        }
                        else
                        {
                            var existingTask = data.Tasks.Where(x => x.ID == task.SurveyTaskID).Single();
                            if (existingTask != null)
                            {
                                data.Tasks.Remove(existingTask);
                                existingTask.Count++;
                                existingTask.Weight += task.Weight;
                                data.Tasks.Add(existingTask);
                            }
                        }

                        totalCount++;
                        totalWeight += task.Weight;
                    }
                }

                responseCount = responses.Count();
            }   

            foreach(var item in data.Tasks)
            {

                // item.CountPercent = ((double)item.Count / totalCount);
                item.CountPercent = ((double)item.Count / responseCount);
                item.WeightPercent = ((double)item.Weight / totalWeight);
                // item.WeightPercent = ((double)item.Weight / (maxWeight * responseCount));
                item.Importance = ((double)item.Weight / item.Count);
            }

            return data; 
        }

    }

    public class SurveySummary
    {
        public List<ResponseData> Tasks { get; set; }

        public SurveySummary()
        {
            Tasks = new List<ResponseData>();
        }
    }

    public class ResponseData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Weight { get; set; }

        public double CountPercent { get; set; }
        public double WeightPercent { get; set; }
        public double Importance { get; set; }
    }
}
