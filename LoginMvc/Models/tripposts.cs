using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginMvc.Models
{
    public class tripposts
    {
       
        public int id { get; set; }
        public String agencyname { get; set; }
        public String triptitle { get; set; }
        public String tripdesctiption { get; set; }
        public String tripdate { get; set; }
        public String tripdestination { get; set; }
        public int active { get; set; }

        [Display(Name = "tripimage")]

        public String tripimage { get; set; }

    }
}