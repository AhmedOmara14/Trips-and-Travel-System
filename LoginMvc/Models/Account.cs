using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginMvc.Models
{
    public partial class Account
    {
        public String id { get; set; }
        public String firstname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
        public String Password { get; set; }
        public String phone { get; set; }
        public String userrole { get; set; }
        public String image { get; set; }


    }
}