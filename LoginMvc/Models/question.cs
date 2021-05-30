using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginMvc.Models
{
    public class question
    {
        public int id { get; set; }

        public int travellerId { get; set; }

        public String que { get; set; }
        public String answer { get; set; }

        public int active { get; set; }

    }
}