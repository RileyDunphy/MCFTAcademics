using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public static class GradeDAL
    {
        static Grade GradeFromRow(IDataReader reader)
        {
            var locked = (bool)reader["lock"];
            var supplemental = (bool)reader["isSupplemental"];
            var given = (DateTime)reader["given"];
            var hoursAttended = (decimal)reader["hoursAttended"];
            var grade = (decimal)reader["grade"];
            // XXX: Switch query to a join and use CourseDAL
            var course = Course.GetCourseById((int)reader["courseId"]);
            // XXX: Preserve the staff stuff too?
            return new Grade(grade, given, locked, hoursAttended, supplemental, course);
        }

        public static IEnumerable<Grade> GetAllGrades()
        {
            SqlConnection connection = null;
            try
            {
                connection = DbConn.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "mcftacademics.dbo.Get_All_Grades";
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                    yield return GradeFromRow(reader);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return grades;//return the list of grades;
        }

        // XXX: This is probably very wrong. CourseStaff is halfway a
        // table for m:n relations, but can also represent temps... which
        // the schema can't allow for. As such, instructors can only be
        // represented by User since it's the only other PK usable, so...
        // (This should really be changed.)
        public static IEnumerable<Grade> GetGradesForInstructor(User staff)
        {
            SqlConnection connection = null;
            try
            {
                connection = DbConn.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "mcftacademics.dbo.Get_Grades_ByStaff";
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                    yield return GradeFromRow(reader);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return grades;//return the list of grades;
        }

        // moved from StudentDAL
        internal static List<Grade> GetGradesForStudent(Student student)
        {
            SqlConnection conn = DbConn.GetConnection();
            List<Grade> grades = new List<Grade>();
            Grade grade = null;
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentGradeById2", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@studentId", student.Id);
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    grade = GradeDAL.GradeFromRow(reader);
                    grades.Add(grade);
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
            return grades;
        }

        internal static Grade GetGradesForStudentInCourse(Course course, Student student)
        {
            SqlConnection conn = DbConn.GetConnection();
            Grade grade = null;
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectGradeByCourseAndStudent", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@courseId", course.Id);
                selectCommand.Parameters.AddWithValue("@studentId", student.Id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    grade = GradeDAL.GradeFromRow(reader);
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
            return grade;//return the grade
        }

        public static bool ToggleGradeLock(int studentId, int courseId)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            try
            {
                conn.Open();
                SqlCommand updateCommand = new SqlCommand("mcftacademics.dbo.ToggleGradeLock", conn);
                updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
                updateCommand.Parameters.AddWithValue("@studentId", studentId);
                updateCommand.Parameters.AddWithValue("@courseId", courseId);
                int rows = updateCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;

                }
            }
            catch (Exception ex)
            {

                result = false;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}
