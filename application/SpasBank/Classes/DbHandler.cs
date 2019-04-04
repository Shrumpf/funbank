using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Dapper

namespace WpfApp2.Classes
{
    public class DbHandler
    {
        private static SqlConnectionStringBuilder builder =
            new SqlConnectionStringBuilder(ConfigurationManager.AppSettings["connectionString"]);

        SqlConnection conn = new SqlConnection(builder.ConnectionString);
        public DbHandler()
        {
           
        }
        //public T database<T>()
        //{
        //    return conn.()
        //}
    }
}
