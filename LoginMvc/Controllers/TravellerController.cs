using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TripsandTravelSystem.Controllers
{
    public class TravellerController : Controller
    {

        SqlCommand sqlCommand = new SqlCommand();
        SqlDataReader dr;
        protected Singleton db = Singleton.Instance;



        [HttpGet]
        public ActionResult ProfileOfTraveller()
        {

           db.ConnsectionString();

            String sql = "SELECT * FROM tripposts WHERE active LIKE'" + 0 + "'";
            SqlCommand cmd = new SqlCommand(sql, db.sqlConnection);

            var model = new List<tripposts>();

            db.sqlConnection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var post = new tripposts();

                post.id = (int)rdr["id"];
                post.agencyname = (string)rdr["agencyname"];
                post.triptitle = (string)rdr["triptitle"];
                post.tripdesctiption = (string)rdr["tripdesctiption"];
                post.tripdate = (string)rdr["tripdate"];
                post.tripdestination = (string)rdr["tripdestination"];


                model.Add(post);
            }

            db.sqlConnection.Close();
            return View(model);
        }
    }
}