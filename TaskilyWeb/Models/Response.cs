using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskilyWeb.Models
{
    public class Response
    {
        public int ID { get; set; }

        public int? SurveyID { get; set; }
        public virtual Survey Survey { get; set; }

        public DateTime? Completed { get; set; }

        public virtual ICollection<Picked> Picked { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}