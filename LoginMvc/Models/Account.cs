using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginMvc.Models
{
    public partial class Account
    {
        public int id { get; set; }
        public String firstname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
        public String password { get; set; }
        public String phone { get; set; }
        public String userrole { get; set; }
        public String image { get; set; }

    }
}