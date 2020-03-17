using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class CourseCodeDAL
    {
        static CourseCode CourseCodeFromRow(IDataReader reader)
        {
            return new CourseCode(reader["courseCode"].ToString(), DateTime.Parse(reader["startDate"].ToString()), DateTime.Parse(reader["endDate"].ToString()), Convert.ToInt32(reader["semester"]));
        }
        public static CourseCode GetNewestCourseCodeById(int id)
        {
            CourseCode courseCode = null;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectNewestCourseCodeById", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    courseCode = CourseCodeFromRow(reader);
                }
            }
            return courseCode;//return the coursecode
        }

        public static int GetIdByCourseCode(string code)
        {
            int id;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectIdByCourseCode", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@code", code);
                //execute the sql statement
                id = Convert.ToInt32(selectCommand.ExecuteScalar());
            }
            return id;
        }

        public static bool AddCourseCode(int id, CourseCode c)
        {
            bool result;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.AddCourseCode", connection);
                insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                insertCommand.Parameters.AddWithValue("@id", id);
                insertCommand.Parameters.AddWithValue("@code", c.Code);
                insertCommand.Parameters.AddWithValue("@startDate", c.From);
                insertCommand.Parameters.AddWithValue("@endDate", c.To);
                insertCommand.Parameters.AddWithValue("@semester", c.Semester);
                int rows = insertCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    result= true;
                }
                else
                {
                    result= false;
                }
            }
            return result;
        }
        public static List<CourseCode> GetAllCourseCodesById(int id)
        {
            List<CourseCode> courseCodes = new List<CourseCode>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectCourseCodesById", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@courseId", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    CourseCode c = CourseCodeFromRow(reader);
                    courseCodes.Add(c);

                }
            }
            return courseCodes;//return the list of coursecodes
        }
        public static List<string> SearchCourseCodes(string code)
        {
            List<string> courseCodes = new List<string>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SearchCourseCodes", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@code", code);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    courseCodes.Add(reader["courseCode"].ToString());

                }
            }
            return courseCodes;//return the list of coursecodes
        }

        public static List<CourseCode> GetCourseCodes(SqlConnection connection, int id)
        {
            List<CourseCode> coursecodes = new List<CourseCode>();
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectCourseCodesById", connection);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@courseId", id);
            //execute the sql statement
            SqlDataReader reader = selectCommand.ExecuteReader();
            //loop through the resultset
            while (reader.Read())
            {
                coursecodes.Add(CourseCodeFromRow(reader));
            }
            return coursecodes;
        }

        public static List<CourseCode> GetCourseCodes(int id)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                return GetCourseCodes(connection, id);
            }
        }
    }
}
