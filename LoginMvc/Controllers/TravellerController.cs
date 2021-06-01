using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using TripsandTravelSystem.Models;

namespace TripsandTravelSystem.Controllers
{
    public class TravellerController : Controller
    {

        SqlCommand sqlCommand = new SqlCommand();
        SqlDataReader dr;
        protected Singleton db = Singleton.Instance;
        List<tripposts> model2 = new List<tripposts>();



        [HttpGet]
        public ActionResult ProfileOfTraveller(string searchBy, string search)
        {
          
                db.ConnsectionString();
                String sql = "SELECT * FROM [dbo].[tripposts] WHERE active LIKE '" + 1 + "'";
                if (searchBy == "agencyname")
                {
                    sql = "SELECT * FROM [dbo].[tripposts] WHERE [agencyname] LIKE'%" + search + "%' AND active LIKE '" + 1 + "'";

                }
                else if (searchBy == "tripdestination")
                {
                    sql = "SELECT * FROM [dbo].[tripposts] WHERE [tripdestination] LIKE'%" + search + "%' AND active LIKE '" + 1 + "'";

                }
                else if (searchBy == "tripdate")
                {
                    sql = "SELECT * FROM [dbo].[tripposts] WHERE [tripdate] LIKE'%" + search + "%' AND active LIKE '" + 1 + "'";

                }
                SqlCommand cmd = new SqlCommand(sql, db.sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDataAdapter.Fill(ds);

                var model = new List<tripposts>();
                model2 = model;

                db.sqlConnection.Open();
                foreach (DataRow rdr in ds.Tables[0].Rows)
                {
                    var post = new tripposts();

                    post.id = (int)rdr["id"];
                    post.agencyname = (string)rdr["agencyname"];
                    post.triptitle = (string)rdr["triptitle"];
                    post.tripdesctiption = (string)rdr["tripdesctiption"];
                    post.tripdate = (string)rdr["tripdate"];
                    post.tripdestination = (string)rdr["tripdestination"];
                    post.tripimage = (string)rdr["tripimage"];


                    model.Add(post);
                }

                ModelState.Clear();
                db.sqlConnection.Close();

                return View(model);
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePro(Account account, HttpPostedFileBase doc)
        {
            db.ConnsectionString();

            if (doc != null)
            {
                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png")
                {
                    var path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);

                    account.image = "~/Content/Images/" + filename;

                    db.sqlConnection.Open();
                    sqlCommand.Connection = db.sqlConnection;
                    string value = "";
                    foreach (string key in Session.Contents)
                    {
                        value = Session[key].ToString();
                    }
                    sqlCommand.CommandText =
                    "UPDATE Account SET firstname=@firstname ,lastname = @lastname,password=@password,email=@email,phone=@phone,image=@image WHERE id='" + value + "'";


                    sqlCommand.Parameters.AddWithValue("@firstname", account.firstname);
                    sqlCommand.Parameters.AddWithValue("@lastname", account.lastname);
                    sqlCommand.Parameters.AddWithValue("@password", account.password);
                    sqlCommand.Parameters.AddWithValue("@email", account.email);
                    sqlCommand.Parameters.AddWithValue("@phone", account.phone);
                    sqlCommand.Parameters.AddWithValue("@image", account.image);

                    sqlCommand.ExecuteReader();
                    db.sqlConnection.Close();

                    return RedirectToAction("TravellerDashbord");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Document size must be less then 5MB');</script>");
                    db.sqlConnection.Close();

                    return RedirectToAction("TravellerDashbord");
                }

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Photo is required');</script>");
            return RedirectToAction("TravellerDashbord");
            db.sqlConnection.Close();

            return View("TravellerDashbord");

        }


        [HttpGet]
        public ActionResult TravellerDashbord()
        {
            db.ConnsectionString();
            string value = "";
            foreach (string key in Session.Contents)
            {
                value = Session[key].ToString();
            }
            String sql = "SELECT * FROM Account WHERE id='" + value + "'";
            SqlCommand cmd = new SqlCommand(sql, db.sqlConnection);

            var model = new Account();
            String img;
            db.sqlConnection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var acc = new Account();

                acc.id = (int)rdr["id"];
                acc.firstname = (string)rdr["firstname"];
                acc.lastname = (string)rdr["lastname"];
                acc.password = (string)rdr["password"];
                acc.email = (string)rdr["email"];
                acc.phone = (string)rdr["phone"];
                acc.image = (string)rdr["image"];
                acc.userrole = (string)rdr["userrole"];

                model = acc;
            }

            db.sqlConnection.Close();

            return View(model);
        }

        
        [HttpGet]
        public ActionResult answersofquestions()
        {
            db.ConnsectionString();

            String sql = "SELECT * FROM question WHERE active LIKE'" + 1 + "'";
            SqlCommand cmd = new SqlCommand(sql, db.sqlConnection);

            var model = new List<question>();

            db.sqlConnection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var post = new question();

                post.id = (int)rdr["id"];
                post.que = (string)rdr["que"];
                post.answer = (string)rdr["answer"];


                model.Add(post);
            }

            db.sqlConnection.Close();
            return View(model);
        }

        [HttpGet]
        public ActionResult showfavoriteposts()
        {
            string value = "";
            foreach (string key in Session.Contents)
            {
                value = Session[key].ToString();
            }
            db.ConnsectionString();

                String sql = "SELECT * FROM tripposts INNER JOIN Cart ON cart.postId = tripposts.id WHERE cart.travellerId ='" + int.Parse(value) + "'";
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
            ProfileOfTraveller("", "");
           return View(model2);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login", "Account");
        }


        public ActionResult savePost(int id)
        {
            db.ConnsectionString();
            string value = "";


            foreach (string key in Session.Contents)
            {
                value = Session[key].ToString();
            }

             db.sqlConnection.Open();
             sqlCommand.Connection = db.sqlConnection;
             sqlCommand.CommandText =
              "INSERT INTO cart VALUES(@postId,@travellerId)";

             sqlCommand.Parameters.Add("@postId", id);
             sqlCommand.Parameters.Add("@travellerId", int.Parse(value));
                
             sqlCommand.ExecuteReader();
          
            db.sqlConnection.Close();
            return Redirect("~/Traveller/TravellerDashbord");
            return View("~/Views/Traveller/TravellerDashbord.cshtml");

        }


    }
}
