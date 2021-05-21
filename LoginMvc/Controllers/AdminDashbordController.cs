using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
          /*  ConnsectionString();
            String sql = "SELECT * FROM Product";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);

            var model = new List<Product>();

            sqlConnection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var student = new Product();
                student.productId = (int)rdr["productId"];
                student.productName = (string)rdr["productName"];
                student.Price = (string)rdr["price"];
                student.count = (int)rdr["count"];

                model.Add(student);
            }*/


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


            return View();

        }


        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
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
                acc.Password = (string)rdr["password"];
                acc.email = (string)rdr["email"];
                acc.phone = (string)rdr["phone"];
                acc.userrole = (string)rdr["userrole"];
     
               
                model.Add(acc);
            }


            return View(model);
        }
    }
}