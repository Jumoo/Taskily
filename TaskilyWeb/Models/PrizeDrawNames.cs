using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskilyWeb.Models
{
    public class PrizeDrawNames
    {
        public int ID { get; set; }

        public int? SurveyID { get; set; }
        public virtual Survey Survey { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public bool contact { get; set; }
    }
}