using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class GradeDAL
    {
        public static Grade GradeFromRow(IDataReader reader)
        {
            return new Grade(Convert.ToInt32(reader["studentId"]),Convert.ToDecimal(reader["grade"]), Convert.ToDateTime(reader["given"]), Convert.ToBoolean(reader["lock"]), Convert.ToDecimal(reader["hoursAttended"]),Convert.ToBoolean(reader["isSupplemental"]),Course.GetCourseById(Convert.ToInt32(reader["courseId"])));
        }

        public static List<Grade> GetAllGrades()
        {
            SqlConnection conn = DbConn.GetConnection();
            List<Grade> grades = new List<Grade>();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectAllGrades", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Grade g = GradeFromRow(reader);
                    grades.Add(g);
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
            return grades;//return the list of grades;
        }
    }
}
