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

namespace LoginMvc.Controllers
{
   

    public class AdminDashbordController : Controller
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        SqlDataReader dr;

        public void ConnsectionString()
        {
            sqlConnection.ConnectionString
                = "data source=localhost; database=TravelDatabase; integrated security = SSPI;";
        }
        // GET: Account
        [HttpGet]
        public ActionResult ShowAdminDashbord()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            ConnsectionString();
            sqlConnection.Open();

            String sql = "DELETE FROM Account WHERE id= @id";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();


            return RedirectToAction("ViewAllUser");

        }
        
        public ActionResult Deletepost(int id)
        {
            ConnsectionString();
            sqlConnection.Open();

            String sql = "DELETE FROM tripposts WHERE id= @id";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();


            return RedirectToAction("ViewAllPosts");

        }


        [HttpGet]
        public ActionResult Edit()
        {

            return View();


        }
        [HttpPost]
        public ActionResult Edit(tripposts post)
        {
            ConnsectionString();
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText =
           "UPDATE tripposts SET agencyname=@agencyname ,triptitle = @triptitle,tripdesctiption=@tripdesctiption,tripdate=@tripdate,tripdestination=@tripdestination)";

            sqlCommand.Parameters.AddWithValue("@agencyname", post.agencyname);
            sqlCommand.Parameters.AddWithValue("@triptitle", post.triptitle);
            sqlCommand.Parameters.AddWithValue("@tripdesctiption", post.tripdesctiption);
            sqlCommand.Parameters.AddWithValue("@tripdate", post.tripdate);
            sqlCommand.Parameters.AddWithValue("@tripdestination", post.tripdestination);

           
            sqlCommand.ExecuteReader();
            return RedirectToAction("ViewAllPosts");
                
              

            
        }


        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        
        [HttpGet]
        public ActionResult AddPosts()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewAllPosts()
        {
            ConnsectionString();
            
            String sql = "SELECT * FROM tripposts WHERE active LIKE'" + 1 + "'";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);

            var model = new List<tripposts>();

            sqlConnection.Open();
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


            return View(model);
        }
        
        [HttpGet]
        public ActionResult ViewAllUser()
        {
            ConnsectionString();
            String sql = "SELECT * FROM Account";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);

            var model = new List<Account>();

            sqlConnection.Open();
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
                acc.userrole = (string)rdr["userrole"];
     
               
                model.Add(acc);
            }


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(Account account, HttpPostedFileBase doc)
        {
            ConnsectionString();

            if (doc != null)
            {
                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png")
                {
                    var path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);

                    account.image = "~/Content/Images/" + filename;

                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText =
                        "INSERT INTO Account VALUES(@firstname,@lastname,@password,@email,@phone,@image,@userrole)";

                    sqlCommand.Parameters.Add("@firstname", account.firstname);
                    sqlCommand.Parameters.Add("@lastname", account.lastname);
                    sqlCommand.Parameters.Add("@password", account.password);
                    sqlCommand.Parameters.Add("@email", account.email);
                    sqlCommand.Parameters.Add("@phone", account.phone);
                    sqlCommand.Parameters.Add("@image", account.image);
                    sqlCommand.Parameters.Add("@userrole", account.userrole);

                    sqlCommand.ExecuteReader();
                    return RedirectToAction("ViewAllUser");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Document size must be less then 5MB');</script>");
                    return RedirectToAction("AddUser");
                }

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Photo is required');</script>");
            return RedirectToAction("AddUser");
            return View("AddUser");
        }


        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPosts(tripposts post, HttpPostedFileBase doc)
        {
            ConnsectionString();

            if (doc != null)
            {
                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png")
                {
                    var path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);

                    post.tripimage = "~/Content/Images/" + filename;

                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText =
                        
                    "INSERT INTO tripposts VALUES(@agencyname,@triptitle,@tripdesctiption,@tripdate,@tripdestination,@tripimage,@active)";

                    post.active = 1;
                    sqlCommand.Parameters.Add("@agencyname", post.agencyname);
                    sqlCommand.Parameters.Add("@triptitle", post.triptitle);
                    sqlCommand.Parameters.Add("@tripdesctiption", post.tripdesctiption);
                    sqlCommand.Parameters.Add("@tripdate", post.tripdate);
                    sqlCommand.Parameters.Add("@tripdestination", post.tripdestination);

                    sqlCommand.Parameters.Add("@tripimage", post.tripimage);
                    sqlCommand.Parameters.Add("@active",post.active);

                    sqlCommand.ExecuteReader();
                    return RedirectToAction("ViewAllPosts");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Document size must be less then 5MB');</script>");
                    return RedirectToAction("AddPosts");
                }

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Photo is required');</script>");
            return RedirectToAction("AddPosts");
            return View("AddPosts");
        }


    }
}