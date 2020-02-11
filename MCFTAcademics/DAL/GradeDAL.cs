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
        }
}
