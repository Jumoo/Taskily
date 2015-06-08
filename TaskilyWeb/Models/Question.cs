using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TaskilyWeb.Models
{
    [DataContract]
    public class Question
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int SurveyID { get; set; }
        
        public virtual Survey Survey { get; set; }

        [Display(Name = "Name", Description="Name of the question")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public QuestionType Type { get; set; }

        [Display(
            Name = "Question Info", 
            Description = "Extra information for multiple choice (one per line)")]
        [DataMember]
        public string Data { get; set; }

        // this might be useful? 
        // public virtual ICollection<Answer> Answers { get; set; }
    }

    public enum QuestionType
    {
        FreeText, Number, MultipleChoice, YesNo
    }
}