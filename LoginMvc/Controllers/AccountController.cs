using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                = "data source=localhost; database=LoginMVC; integrated security = SSPI;";
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
                "SELECT * FROM account WHERE email='"+account.email+
                "'and Password='"+account.Password+"'";

            dr = sqlCommand.ExecuteReader();

            if (dr.Read())
            {
                sqlConnection.Close();

                return Redirect("~/showmember/Login");
                return View("~/Views/showmember/Login.cshtml");

            }
            else
            {
                sqlConnection.Close();
                return View("fail");
            }

        }

      


    }
}