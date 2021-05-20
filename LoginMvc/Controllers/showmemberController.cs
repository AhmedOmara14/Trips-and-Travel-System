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
   

    public class showmemberController : Controller
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
            ConnsectionString();
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
            }


            return View(model);
        }

        public ActionResult Delete(int id)
        {
            ConnsectionString();
            sqlConnection.Open();

            String sql = "DELETE FROM Product WHERE productId= @productId";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@productId", id);
            cmd.ExecuteNonQuery();


            return Redirect("~/showmember/Login");
            return View("~/Views/showmember/Login.cshtml");

        }





    }
}