using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class CourseCodeDAL
    {
        public static CourseCode getNewestCourseCodeById(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            conn.Open(); //open the connection
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectNewestCourseCodeById", conn);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@id", id);
            //execute the sql statement
            SqlDataReader reader = selectCommand.ExecuteReader();
            CourseCode courseCode = new CourseCode();
            //loop through the resultset
            while (reader.Read())
            {
                courseCode = new CourseCode(reader["courseCode"].ToString(), DateTime.Parse(reader["startDate"].ToString()), DateTime.Parse(reader["endDate"].ToString()));
            }
            conn.Close();//don't forget to close the connection
            return courseCode;//return the course
        }

        public static int getIdByCourseCode(string code)
        {
            SqlConnection conn = DbConn.GetConnection();
            conn.Open(); //open the connection
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectIdByCourseCode", conn);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@code", code);
            //execute the sql statement
            int id = Convert.ToInt32(selectCommand.ExecuteScalar());
            conn.Close();
            return id;
        }

        public static bool addCourseCode(int id, string code)
        {
            SqlConnection conn = DbConn.GetConnection();
            conn.Open();
            SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.AddCourseCode", conn);
            insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
            insertCommand.Parameters.AddWithValue("@id", id);
            insertCommand.Parameters.AddWithValue("@code", code);
            int rows = insertCommand.ExecuteNonQuery();
            conn.Close();
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
