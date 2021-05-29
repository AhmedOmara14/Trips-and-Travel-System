using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TripsandTravelSystem.Controllers
{
    public sealed class Singleton
    {
      public SqlConnection sqlConnection = new SqlConnection();

       
      private Singleton()
        {
        }
        private static readonly Lazy<Singleton> lazy = new Lazy<Singleton>(() => new Singleton());
        public static Singleton Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        public void ConnsectionString()
        {
            sqlConnection.ConnectionString
                = "data source=localhost; database=TravelDatabase; integrated security = SSPI;";
        }
    }
}