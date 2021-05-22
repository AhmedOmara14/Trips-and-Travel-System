using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
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

      


    }
}