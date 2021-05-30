using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using TripsandTravelSystem.Factory;

namespace TripsandTravelSystem.Controllers
{
    public class AgencyController : Controller, Logininterface
    {
        protected Singleton db = Singleton.Instance;
        SqlCommand sqlCommand = new SqlCommand();

       
        public ActionResult Profile()
        {
            db.ConnsectionString();
            string value = "";
            foreach (string key in Session.Contents)
            {
                value = Session[key].ToString();
                Debug.WriteLine("is is :"+value);
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
                acc.lastname = (object)rdr["lastname"];
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

        public ActionResult register(Account account, HttpPostedFileBase doc)
        {
            throw new NotImplementedException();
        }

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
                        Debug.WriteLine(value);
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

                    return RedirectToAction("ProfileOfAgency");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Document size must be less then 5MB');</script>");
                    db.sqlConnection.Close();

                    return RedirectToAction("ProfileOfAgency");
                }

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Photo is required');</script>");
            return RedirectToAction("ProfileOfAgency");
            db.sqlConnection.Close();

            return View("ProfileOfAgency");

        }

        [HttpGet]
        public ActionResult createnewpost()
        {
            return View();
        }

        public ActionResult AddPosts(tripposts post, HttpPostedFileBase doc)
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

                    post.tripimage = "~/Content/Images/" + filename;

                    db.sqlConnection.Open();
                    sqlCommand.Connection = db.sqlConnection;
                    sqlCommand.CommandText =

                    "INSERT INTO tripposts VALUES(@agencyname,@triptitle,@tripdesctiption,@tripdate,@tripdestination,@tripimage,@active)";

                    post.active = 0;
                    sqlCommand.Parameters.Add("@agencyname", post.agencyname);
                    sqlCommand.Parameters.Add("@triptitle", post.triptitle);
                    sqlCommand.Parameters.Add("@tripdesctiption", post.tripdesctiption);
                    sqlCommand.Parameters.Add("@tripdate", post.tripdate);
                    sqlCommand.Parameters.Add("@tripdestination", post.tripdestination);

                    sqlCommand.Parameters.Add("@tripimage", post.tripimage);
                    sqlCommand.Parameters.Add("@active", post.active);

                    sqlCommand.ExecuteReader();
                    db.sqlConnection.Close();

                    return RedirectToAction("myposts");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Document size must be less then 5MB');</script>");
                    db.sqlConnection.Close();

                    return RedirectToAction("createnewpost");
                }

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Photo is required');</script>");
            return RedirectToAction("createnewpost");
            db.sqlConnection.Close();

            return View("createnewpost");
        }

   
        [HttpGet]
        public ActionResult myposts()
        {
            db.ConnsectionString();

            String sql = "SELECT * FROM tripposts WHERE active LIKE'" + 1 + "'";
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

        public ActionResult Deletepost(int id)
        {
            db.ConnsectionString();
            db.sqlConnection.Open();

            String sql = "DELETE FROM tripposts WHERE id= @id";
            SqlCommand cmd = new SqlCommand(sql, db.sqlConnection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            db.sqlConnection.Close();

            return RedirectToAction("createnewpost");

        }

        [HttpPost]
        public ActionResult Edit(tripposts post)
        {
            db.ConnsectionString();
            db.sqlConnection.Open();
            sqlCommand.Connection = db.sqlConnection;
            Debug.WriteLine("id" + post.id);
            sqlCommand.CommandText =
           "UPDATE tripposts SET agencyname=@agencyname ,triptitle = @triptitle,tripdesctiption=@tripdesctiption,tripdate=@tripdate,tripdestination=@tripdestination WHERE id='" + post.id + "'";

            sqlCommand.Parameters.AddWithValue("@agencyname", post.agencyname);
            sqlCommand.Parameters.AddWithValue("@triptitle", post.triptitle);
            sqlCommand.Parameters.AddWithValue("@tripdesctiption", post.tripdesctiption);
            sqlCommand.Parameters.AddWithValue("@tripdate", post.tripdate);
            sqlCommand.Parameters.AddWithValue("@tripdestination", post.tripdestination);


            sqlCommand.ExecuteReader();
            db.sqlConnection.Close();
            return RedirectToAction("myposts");

        }
        

        [HttpPost]
        public ActionResult reply(question post)
        {
            db.ConnsectionString();
            db.sqlConnection.Open();
            sqlCommand.Connection = db.sqlConnection;
            Debug.WriteLine("id : " + post.answer+":");
            sqlCommand.CommandText =
           "UPDATE question SET answer=@answer,active="+1+" WHERE id='" + post.id + "'";

            sqlCommand.Parameters.AddWithValue("@answer", post.answer);


            sqlCommand.ExecuteReader();
            db.sqlConnection.Close();
            return RedirectToAction("Receivedquestions");


        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); 
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public ActionResult Receivedquestions()
        {

            db.ConnsectionString();

            String sql = "SELECT * FROM question WHERE active LIKE'" + 0 + "'";
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



    }
}