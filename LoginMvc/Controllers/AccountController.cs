using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TripsandTravelSystem.Controllers;
using TripsandTravelSystem.Factory;

namespace LoginMvc.Controllers
{
    public class AccountController : Controller, Logininterface
    {
        SqlCommand sqlCommand = new SqlCommand();
        SqlDataReader dr;
        protected Singleton db = Singleton.Instance;

        [HttpGet]
        public ActionResult Login(string searchBy, string search)
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
        public ActionResult verify(Account account)
        {
            db.ConnsectionString();
            db.sqlConnection.Open();
            sqlCommand.Connection = db.sqlConnection;
            sqlCommand.CommandText =
                "SELECT * FROM Account WHERE email LIKE'" + account.email +
                "'and password LIKE '" + account.password + "'";

            dr = sqlCommand.ExecuteReader();



            if (dr.Read())
            {
                Session["UserID"] = dr["id"].ToString();

                Debug.WriteLine("user role2 : " + dr["userrole"].ToString());
                String val = dr["userrole"].ToString();


                if (String.Equals(val, "admin"))
                {
                    db.sqlConnection.Close();

                    return Redirect("~/AdminDashbord/ShowAdminDashbord");
                    return View("~/Views/AdminDashbord/ShowAdminDashbord.cshtml");

                }
                else if (String.Equals(val, "agency"))
                {
                    db.sqlConnection.Close();

                    return Redirect("~/Agency/Profile");
                    return View("~/Views/Agency/Profile.cshtml");
                }
                else if (String.Equals(val, "traveller"))
                {
                    db.sqlConnection.Close();

                    return Redirect("~/Traveller/ProfileOfTraveller");
                    return View("~/Views/Traveller/ProfileOfTraveller.cshtml");
                }
                else
                {
                    db.sqlConnection.Close();

                    return Redirect("~/showmember/Login");
                }
                db.sqlConnection.Close();

                // return Redirect("~/showmember/Login");

            }
            else
            {
                db.sqlConnection.Close();
                return Content("<script language='javascript' type='text/javascript'>alert('Check username or password!!');</script>");

                return Redirect("~/showmember/Login");
                return View("~/Views/showmember/Login.cshtml");
            }

        }

        public ActionResult register(Account account, HttpPostedFileBase doc)
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
                    sqlCommand.CommandText =
                        "INSERT INTO Account VALUES(@firstname,@password,@email,@phone,@image,@userrole,@lastname)";

                    sqlCommand.Parameters.Add("@firstname", account.firstname);
                    sqlCommand.Parameters.Add("@password", account.password);
                    sqlCommand.Parameters.Add("@email", account.email);
                    sqlCommand.Parameters.Add("@phone", account.phone);
                    sqlCommand.Parameters.Add("@image", account.image);
                    sqlCommand.Parameters.Add("@userrole", "traveller");
                    sqlCommand.Parameters.Add("@lastname", account.lastname);

                    sqlCommand.ExecuteReader();
                    db.sqlConnection.Close();

                    return Redirect("Login");
                }
                else
                {
                    db.sqlConnection.Close();
                    return Content("<script language='javascript' type='text/javascript'>alert('Document size must be less then 5MB');</script>");
                    return RedirectToAction("Login");
                }

            }
            db.sqlConnection.Close();

            return Content("<script language='javascript' type='text/javascript'>alert('Photo is required');</script>");
            return RedirectToAction("Login");
            return View("Login");
        }

        ActionResult Logininterface.Profile()
        {
            throw new NotImplementedException();
        }

        public ActionResult UpdatePro(Account account, HttpPostedFileBase doc)
        {
            throw new NotImplementedException();
        }

        public ActionResult AddPosts(tripposts post, HttpPostedFileBase doc)
        {
            throw new NotImplementedException();
        }
    }
}