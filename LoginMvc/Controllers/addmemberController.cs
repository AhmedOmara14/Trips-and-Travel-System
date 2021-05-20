using LoginMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginMvc.Controllers
{
    public class addmemberController : Controller
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        SqlDataReader dr;

        public void ConnsectionString()
        {
            sqlConnection.ConnectionString
                = "data source=localhost; database=LoginMVC; integrated security = SSPI;";
        }
        // GET: addmember
        public ActionResult addProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult add(Product product)
        {
            ConnsectionString();
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText =
                "INSERT INTO Product VALUES(@productName,@price,@count)";

            sqlCommand.Parameters.Add("@productName", product.productName);
            sqlCommand.Parameters.Add("@price", product.Price);
            sqlCommand.Parameters.Add("@count", product.count);

            dr = sqlCommand.ExecuteReader();

           
                return Redirect("~/showmember/Login");
                return View("~/Views/showmember/Login.cshtml");

           

        }
    }
}