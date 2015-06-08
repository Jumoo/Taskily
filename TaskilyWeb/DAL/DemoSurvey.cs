using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TaskilyWeb.Models; 

namespace TaskilyWeb.DAL
{
    public class DemoSurvey
    {
        /// <summary>
        ///  creates a demo survey for an organisation
        /// </summary>
        /// <param name="organisationId"></param>
        public static void CreateDemo(int organisationId)
        {

            using (TasklyDbContext db = new TasklyDbContext())
            {

                var survey = new Survey
                {
                    OrganisationID = organisationId,
                    Name = "Demo Survey",
                    EndUrl = "/admin/",
                    Active = true,
                    UID = TaskilyHelper.UniqueCode(),
                    WelcomeTitle = "Hello",
                    WelcomeMessage = "<p>Welcome to the taskily demo survey</p></p>This survey is designed to show you how taskily works</p>",
                    CompleteTitle = "Thanks",
                    CompleteMessage = "Thank you for completing this survey",
                    TaskCount = 5,

                    TasksHeading = "US States",
                    TasksSubHeading = "Tell us the 5 states you like most",

                    OrderHeading = "US States",
                    OrderSubHeading = "Which one do you like the most",
                    OrderText = "Now put the five states you picked in order, with your favorite one at the top",

                };

                db.Surveys.Add(survey);
                db.SaveChanges();

                // add the tasks
                var tasks = new List<SurveyTask>
                {
                    new SurveyTask { Name = "Alabama", Description = "Montgomery", Active = true, survey = survey},
                    new SurveyTask { Name = "Alaska", Description = "Juneau", Active = true, survey = survey},
                    new SurveyTask { Name = "Arizona", Description = "Phoenix", Active = true, survey = survey},
                    new SurveyTask { Name = "Arkansas", Description = "Little Rock", Active = true, survey = survey},
                    new SurveyTask { Name = "California", Description = "Sacramento", Active = true, survey = survey},
                    new SurveyTask { Name = "Colorado", Description = "Denver", Active = true, survey = survey},
                    new SurveyTask { Name = "Connecticut", Description = "Hartford", Active = true, survey = survey},
                    new SurveyTask { Name = "Delaware", Description = "Dover", Active = true, survey = survey},
                    new SurveyTask { Name = "Florida", Description = "Tallahassee", Active = true, survey = survey},
                    new SurveyTask { Name = "Georgia", Description = "Atlanta", Active = true, survey = survey},
                    new SurveyTask { Name = "Hawaii", Description = "Honolulu", Active = true, survey = survey},
                    new SurveyTask { Name = "Idaho", Description = "Boise", Active = true, survey = survey},
                    new SurveyTask { Name = "Illinois", Description = "Springfield", Active = true, survey = survey},
                    new SurveyTask { Name = "Indiana", Description = "Indianapolis", Active = true, survey = survey},
                    new SurveyTask { Name = "Iowa", Description = "Des Moines", Active = true, survey = survey},
                    new SurveyTask { Name = "Kansas", Description = "Topeka", Active = true, survey = survey},
                    new SurveyTask { Name = "Kentucky", Description = "Frankfort", Active = true, survey = survey},
                    new SurveyTask { Name = "Louisiana", Description = "Baton Rouge", Active = true, survey = survey},
                    new SurveyTask { Name = "Maine", Description = "Augusta", Active = true, survey = survey},
                    new SurveyTask { Name = "Maryland", Description = "Annapolis", Active = true, survey = survey},
                    new SurveyTask { Name = "Massachusetts", Description = "Boston", Active = true, survey = survey},
                    new SurveyTask { Name = "Michigan", Description = "Lansing", Active = true, survey = survey},
                    new SurveyTask { Name = "Minnesota", Description = "Saint Paul", Active = true, survey = survey},
                    new SurveyTask { Name = "Mississippi", Description = "Jackson", Active = true, survey = survey},
                    new SurveyTask { Name = "Missouri", Description = "Jefferson City", Active = true, survey = survey},
                    new SurveyTask { Name = "Montana", Description = "Helena", Active = true, survey = survey},
                    new SurveyTask { Name = "Nebraska", Description = "Lincoln", Active = true, survey = survey},
                    new SurveyTask { Name = "Nevada", Description = "Carson City", Active = true, survey = survey},
                    new SurveyTask { Name = "New Hampshire", Description = "Concord", Active = true, survey = survey},
                    new SurveyTask { Name = "New Jersey", Description = "Trenton", Active = true, survey = survey},
                    new SurveyTask { Name = "New Mexico", Description = "Santa Fe", Active = true, survey = survey},
                    new SurveyTask { Name = "New York", Description = "Albany", Active = true, survey = survey},
                    new SurveyTask { Name = "North Carolina", Description = "Raleigh", Active = true, survey = survey},
                    new SurveyTask { Name = "North Dakota", Description = "Bismarck", Active = true, survey = survey},
                    new SurveyTask { Name = "Ohio", Description = "Columbus", Active = true, survey = survey},
                    new SurveyTask { Name = "Oklahoma", Description = "Oklahoma City", Active = true, survey = survey},
                    new SurveyTask { Name = "Oregon", Description = "Salem", Active = true, survey = survey},
                    new SurveyTask { Name = "Pennsylvania", Description = "Harrisburg", Active = true, survey = survey},
                    new SurveyTask { Name = "Rhode Island", Description = "Providence", Active = true, survey = survey},
                    new SurveyTask { Name = "South Carolina", Description = "Columbia", Active = true, survey = survey},
                    new SurveyTask { Name = "South Dakota", Description = "Pierre", Active = true, survey = survey},
                    new SurveyTask { Name = "Tennessee", Description = "Nashville", Active = true, survey = survey},
                    new SurveyTask { Name = "Texas", Description = "Austin", Active = true, survey = survey},
                    new SurveyTask { Name = "Utah", Description = "Salt Lake City", Active = true, survey = survey},
                    new SurveyTask { Name = "Vermont", Description = "Montpelier", Active = true, survey = survey},
                    new SurveyTask { Name = "Virginia", Description = "Richmond", Active = true, survey = survey},
                    new SurveyTask { Name = "Washington", Description = "Olympia", Active = true, survey = survey},
                    new SurveyTask { Name = "West Virginia", Description = "Charleston", Active = true, survey = survey},
                    new SurveyTask { Name = "Wisconsin", Description = "Madison", Active = true, survey = survey},
                    new SurveyTask { Name = "Wyoming", Description = "Cheyenne", Active = true, survey = survey}
                };
                tasks.ForEach(t => db.Tasks.Add(t));


                var Questions = new List<Question> {
                    new Question { Name = "Age", Text = "How old are you?", Type = QuestionType.MultipleChoice, Data = "0-18\n19-30\n31-45\n46-75\n75+", Survey = survey},
                    new Question { Name = "Feedback", Text = "Any other feedback?", Type = QuestionType.FreeText, Survey = survey }
                };
                Questions.ForEach(q => db.Questions.Add(q));

                db.SaveChanges();
            }
        }
    }

    
}