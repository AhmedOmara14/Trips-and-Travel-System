using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginMvc.Models
{
    public class Likes
    {
        public int postsId { get; set; }
        public int userId { get; set; }
        public int activelike { get; set; }
        public int activedislike { get; set; }  
    }
}