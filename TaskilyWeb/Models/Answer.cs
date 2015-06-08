using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskilyWeb.Models
{
    public class Answer
    {
        public int ID { get; set; }

        public int ResponseID { get; set; }
        public virtual Response Response { get; set; }

        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }

        //
        // some answers are numbers some are text
        // but we might just store both as text?
        //
        public int AnswerNo { get; set;  }
        public string AnswerText { get; set; }
    }
}