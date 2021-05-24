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

namespace LoginMvc.Controllers
{
    public class AccountController : Controller
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
        public ActionResult Login()
        {
            return View();
        }
       

        [HttpPost]
        public ActionResult verify(Account account)
        {
            ConnsectionString();
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText =
                "SELECT * FROM Account WHERE email LIKE'" + account.email+
                "'and password LIKE '"+account.password+"'";

            dr = sqlCommand.ExecuteReader();


           
            if (dr.Read())
            {
                Session["UserID"] = dr["id"].ToString();

                Debug.WriteLine("user role2 : " + dr["userrole"].ToString());
                String val = dr["userrole"].ToString();


                if (String.Equals(val, "admin"))
                {
                    return Redirect("~/AdminDashbord/ShowAdminDashbord");
                    return View("~/Views/AdminDashbord/ShowAdminDashbord.cshtml");

                }
                else if (String.Equals(val,"agency"))
                {
                    Debug.WriteLine("user role : hamada");
                    return Redirect("~/showmember/Login");
                    return View("~/Views/showmember/Login.cshtml");
                }
                else if (String.Equals(val, "traveller"))
                {
                    return Redirect("~/showmember/Login");
                    return View("~/Views/showmember/Login.cshtml");
                    Debug.WriteLine("user role111 : traveller");
                }
                else
                {
                    return View("fail");
                }
                sqlConnection.Close();

                // return Redirect("~/showmember/Login");

            }
            else
            {
                sqlConnection.Close();
                return View("fail");
            }

        }


        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(Account account, HttpPostedFileBase doc)
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
                    sqlCommand.Parameters.Add("@userrole", "traveller");

                    sqlCommand.ExecuteReader();
                    return Redirect("~/showmember/Login");
                    return View("~/Views/showmember/Login.cshtml");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Document size must be less then 5MB');</script>");
                    return RedirectToAction("Login");
                }

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Photo is required');</script>");
            return RedirectToAction("Login");
            return View("Login");
        }


    }
}