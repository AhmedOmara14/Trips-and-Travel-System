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

            return View();
        }

     
        [HttpPost]
        public ActionResult ProfileOfTraveller(question post)
        {
            string value = "";
            foreach (string key in Session.Contents)
            {
                value = Session[key].ToString();
            }
            db.ConnsectionString();

            db.sqlConnection.Open();
            sqlCommand.Connection = db.sqlConnection;
            sqlCommand.CommandText =

                 "INSERT INTO question VALUES(@travellerId,@que,@answer,@active)";

                    post.travellerId =int.Parse(value);
                    post.active = 0; 
                    sqlCommand.Parameters.Add("@travellerId", post.travellerId);
                    sqlCommand.Parameters.Add("@que", post.que);
                    sqlCommand.Parameters.Add("@answer", "");
                    sqlCommand.Parameters.Add("@active", post.active);


            sqlCommand.ExecuteReader();
            db.sqlConnection.Close();

           return View();
        }
                
           
        

    }
}