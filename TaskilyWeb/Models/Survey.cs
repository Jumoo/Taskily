using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace TaskilyWeb.Models
{
    public class Survey
    {
        public int ID { get; set; }

        // a string - so we can make it harder to guess the survey id
        [StringLength(24)]
        public string UID { get; set; }
        
        public int OrganisationID { get; set; }
        public virtual Organisation organisation { get; set; }

        [Display(Name = "Survey name")]
        public string Name { get; set; }

        [Display(Name = "Number of tasks to pick")]
        [Required]
        public int TaskCount { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Make results public")]
        public bool PublicResults { get; set; }

        /* prize draw stuff */
        [Display(Name = "Run a prize draw")]
        public bool Draw { get; set; }

        [Display(Name = "Prize draw title")]
        public string DrawTitle { get; set; }

        [Display(Name = "Prize draw message")]
        [AllowHtml]
        public string DrawMessage { get; set; }

        [Display(Name = "Marketing opt-in")]
        public bool MarketingOptIn { get; set; }

        [Display(Name = "Marketing message")]
        [AllowHtml]
        public string MarketingMessage { get; set; }

        [Display(Name = "Welcome title")]
        public string WelcomeTitle { get; set; }

        [Display(Name = "Welcome message")]
        [AllowHtml]
        [MaxLength]
        public string WelcomeMessage { get; set; }

        [Display(Name = "Completed title")]
        public string CompleteTitle { get; set; }

        [Display(Name = "Completed message")]
        [AllowHtml]
        [MaxLength]
        public string CompleteMessage { get; set; }

        [Display(Name = "End URL")]
        public string EndUrl { get; set; }

        public virtual ICollection<SurveyTask> Tasks { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Response> Responses { get; set; }

        public virtual ICollection<PrizeDrawNames> Draws { get; set; }

        // advanced customization stuff.
        [Display(Name = "CSS Stylesheet")]
        public string CssFile { get; set; }

        [AllowHtml]
        [MaxLength]
        [Display(Name = "Header HTML")]
        public string HeaderHtml { get; set; }
        
        [AllowHtml]
        [MaxLength]
        [Display(Name = "Footer HTML")]
        public string FooterHtml { get; set; }

        [Display(Name = "Tasks Heading")]
        public string TasksHeading { get; set; }
        [Display(Name = "Tasks Sub-Heading")]
        public string TasksSubHeading { get; set; }

        [Display(Name = "Order Heading")]
        public string OrderHeading { get; set; }
        [Display(Name = "Order Sub-Heading")]
        public string OrderSubHeading { get; set; }
        
        [AllowHtml]
        [MaxLength]
        [Display(Name = "Order Text")]
        public string OrderText { get; set; }

        [Display(Name = "Question Heading")]
        public string QuestionHeading { get; set; }
        [Display(Name = "Question Sub-Heading")]
        public string QuestionSubHeading { get; set; }

        [AllowHtml]
        [MaxLength]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }
    }
}