using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace TaskilyWeb.Models
{

    public enum OrgAccountType
    {
        free, standard, unlimited
    }

    public class Organisation
    {
        public int ID { get; set; }

        [Display(Name = "Organisation")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }
        public string Address { get; set; }

        [Display(Name = "Account Type")]
        public OrgAccountType AccountType { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? TrialDate { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }
    }
}