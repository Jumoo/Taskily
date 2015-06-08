using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace TaskilyWeb.Models
{
    [DataContract]
    public class SurveyTask
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int SurveyID { get; set; }

        public virtual Survey survey { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

    }
}