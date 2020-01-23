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
            String connString = "Data Source=mcft-academics.cnh1gfldtky9.us-east-1.rds.amazonaws.com;Initial Catalog=mcftacademics;User ID=admin;Password=JoshVision2020";
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
    }
}
