using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class DbConn
    {
        public static SqlConnection GetConnection()
        {
            //get the connection string
            String connString = "Data Source=ec2-54-172-213-145.compute-1.amazonaws.com;Persist Security Info=True;User ID=dev;Password=MarcuVision2020";
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
    }
}
