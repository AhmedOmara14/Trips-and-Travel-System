using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TripsandTravelSystem.Factory
{
    interface Logininterface
    {
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        ActionResult register(Account account, HttpPostedFileBase doc);
        

    }
}
