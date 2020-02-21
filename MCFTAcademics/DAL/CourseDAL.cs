using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class CourseDAL
    {
        internal static Course CourseFromRow(IDataReader reader)
        {
            return new Course(Convert.ToInt32(reader["courseId"]), reader["name"].ToString(),1, Convert.ToDecimal(reader["credit"]), reader["Description"].ToString(), Convert.ToInt32(reader["lectureHours"]), Convert.ToInt32(reader["labHours"]), Convert.ToInt32(reader["examHours"]), Convert.ToInt32(reader["totalHours"]), Convert.ToDecimal(reader["revisionNumber"]), reader["program"].ToString(), Convert.ToBoolean(reader["accreditation"]));
        }
        public static List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.Get_AllCoursesANDCourseCodes", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Course c = CourseFromRow(reader);
                    courses.Add(c);
                }
            }
            return courses;//return the list of courses
        }

        public static List<Course> GetCoursesByInstructor(int userid)
        {
            List<Course> courses = new List<Course>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectCoursesByInstructor", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@userid", userid);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Course c = CourseFromRow(reader);
                    courses.Add(c);

                }
            }
            return courses;//return the list of courses
        }
        public static Course GetCourseById(int id)
        {
            Course course = null;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectCourseByid", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    course = CourseFromRow(reader);
                }
            }
            return course;//return the course
        }
        
        // After refactoring to not hold some stuff like staff inside of
        // itself, we're taking them as arguments (since you often need
        // to change the others too). We could also make it so setting them
        // to null doesn't change them as well.
        // XXX: Not sure this is the best solution. We'll see if it works though.
        public static bool UpdateCourse(Course c, Staff leadStaff, Staff supportStaff, IEnumerable<Prerequisite> prerequisites)
        {
            bool result = false;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction("UpdateCourse for " + c.Id);
                try
                {
                    SqlCommand updateCommand = new SqlCommand("mcftacademics.dbo.UpdateCourseById", connection);
                    updateCommand.Transaction = transaction;
                    updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    updateCommand.Parameters.AddWithValue("@id", c.Id);
                    updateCommand.Parameters.AddWithValue("@name", c.Name);
                    updateCommand.Parameters.AddWithValue("@credit", c.Credit);
                    updateCommand.Parameters.AddWithValue("@description", c.Description);
                    updateCommand.Parameters.AddWithValue("@lectureHours", c.LectureHours);
                    updateCommand.Parameters.AddWithValue("@labHours", c.LabHours);
                    updateCommand.Parameters.AddWithValue("@examHours", c.ExamHours);
                    updateCommand.Parameters.AddWithValue("@revisionNumber", c.RevisionNumber);
                    updateCommand.Parameters.AddWithValue("@program", c.Program);
                    updateCommand.Parameters.AddWithValue("@accreditation", c.Accreditation);
                    int rows = updateCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        //Drop existing staff (so they're not multiple instructors) 
                        //and add back the lead staff and support if there is one
                        StaffDAL.DropStaff(connection, c.Id, transaction);
                        StaffDAL.AddStaff(connection, c.Id, leadStaff, transaction);
                        if (supportStaff != null)
                        {
                            StaffDAL.AddStaff(connection, c.Id, supportStaff, transaction);
                        }
                        PrerequisiteDAL.DropPrereqs(connection, c.Id, transaction);
                        foreach (Prerequisite prereq in prerequisites)
                        {
                            PrerequisiteDAL.AddPrereq(connection, prereq, transaction);
                        }
                        transaction.Commit();
                        result = true;
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // if THAT fails
                        throw;
                    }
                    throw;
                }
            }
            return result;
        }

        //  Similar to Update with args here...
        public static int AddCourse(Course c, Staff leadStaff, Staff supportStaff, IEnumerable<Prerequisite> prerequisites)
        {
            int id;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction("AddCourse");
                try
                {
                    SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertCourse", connection);
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    insertCommand.Parameters.AddWithValue("@name", c.Name);
                    insertCommand.Parameters.AddWithValue("@credit", c.Credit);
                    insertCommand.Parameters.AddWithValue("@description", c.Description);
                    insertCommand.Parameters.AddWithValue("@lectureHours", c.LectureHours);
                    insertCommand.Parameters.AddWithValue("@labHours", c.LabHours);
                    insertCommand.Parameters.AddWithValue("@examHours", c.ExamHours);
                    insertCommand.Parameters.AddWithValue("@revisionNumber", c.RevisionNumber);
                    insertCommand.Parameters.AddWithValue("@program", c.Program);
                    insertCommand.Parameters.AddWithValue("@accreditation", c.Accreditation);
                    int rows = insertCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectLastCourseInsert", connection);
                        selectCommand.Transaction = transaction;
                        id = Convert.ToInt32(selectCommand.ExecuteScalar());
                        //Drop existing staff (so they're not multiple instructors) 
                        //and add back the lead staff and support if there is one
                        StaffDAL.DropStaff(connection, id, transaction);
                        StaffDAL.AddStaff(connection, id, leadStaff, transaction);
                        if (supportStaff != null)
                        {
                            StaffDAL.AddStaff(connection, id, supportStaff, transaction);
                        }
                        PrerequisiteDAL.DropPrereqs(connection, id, transaction);
                        foreach (Prerequisite prereq in prerequisites)
                        {
                            prereq.CourseId = id;
                            PrerequisiteDAL.AddPrereq(connection, prereq, transaction);
                        }
                        transaction.Commit();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // if THAT fails
                        throw;
                    }
                    throw;
                }
            }
            return id;
        }
    }
}
