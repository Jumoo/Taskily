using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskilyWeb.Models
{
    public class Picked
    {
        public int ID { get; set; }

        public int ResponseID { get; set; }
        public virtual Response Response { get; set; }

        public int SurveyTaskID { get; set; }
        public virtual SurveyTask Task { get; set; }

        public int Weight { get; set; }
    }
}