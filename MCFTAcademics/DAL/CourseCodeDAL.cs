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
            SqlConnection conn = DbConn.GetConnection();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectNewestCourseCodeById", conn);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();//don't forget to close the connection
            }
            return courseCode;//return the coursecode
        }

        public static int GetIdByCourseCode(string code)
        {
            SqlConnection conn = DbConn.GetConnection();
            int id;
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectIdByCourseCode", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@code", code);
                //execute the sql statement
                id = Convert.ToInt32(selectCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return id;
        }

        public static bool AddCourseCode(int id, CourseCode c)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            try
            {
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.AddCourseCode", conn);
                insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                insertCommand.Parameters.AddWithValue("@id", id);
                insertCommand.Parameters.AddWithValue("@code", c.Code);
                insertCommand.Parameters.AddWithValue("@startDate", c.From);
                insertCommand.Parameters.AddWithValue("@endDate", c.To);
                insertCommand.Parameters.AddWithValue("@semester", c.Semester);
                int rows = insertCommand.ExecuteNonQuery();
                conn.Close();
                if (rows > 0)
                {
                    result= true;
                }
                else
                {
                    result= false;
                }
            }
            catch(Exception ex)
            {
                result= false;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        public static List<CourseCode> GetAllCourseCodesById(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            List<CourseCode> courseCodes = new List<CourseCode>();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectCourseCodesById", conn);
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
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return courseCodes;//return the list of courses
        }
    }
}
