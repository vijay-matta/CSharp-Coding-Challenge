using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;




namespace LoanManagementSystem.DBUtil
{
    public static class DBUtil
    {
       //static DBUtil()
       //{
       //    var builder = new ConfigurationBuilder()
       //        .SetBasePath(Directory.GetCurrentDirectory())
       //        .AddJsonFile("C:\\Users\\Vijay\\source\\repos\\LoanManagementSystemSolution\\LoanManagementSystem\\DBUtil\\appsettings.json",
       //        optional: true, reloadOnChange: true);
       //        _configuration = builder.Build();
       //}
        public static SqlConnection GetDBConn()
        {
            // **** MAM YOU CAN JUST CHANGE THE PATH HERE I HAVE DIRECTLY GIVEN THE PATH HERE IN MY FAVOUR!!
               string connectionString = "Data Source = VJ-S\\SQLEXPRESS; Initial Catalog = LoanManagementSystem; Integrated Security = True" ;
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            
        }
    }
}
