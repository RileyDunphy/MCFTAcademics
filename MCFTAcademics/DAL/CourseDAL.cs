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
        static Course CourseFromRow(IDataReader reader, List<Prerequisite> prereqs)
        {
            return new Course(Convert.ToInt32(reader["courseId"]), reader["name"].ToString(), Convert.ToDecimal(reader["credit"]), reader["Description"].ToString(), Convert.ToInt32(reader["lectureHours"]), Convert.ToInt32(reader["labHours"]), Convert.ToInt32(reader["examHours"]), Convert.ToInt32(reader["totalHours"]), Convert.ToDecimal(reader["revisionNumber"]), reader["program"].ToString(), Convert.ToBoolean(reader["accreditation"]), prereqs);
        }
        public static List<Course> GetAllCourses()
        {
            SqlConnection conn = DbConn.GetConnection();
            List<Course> courses = new List<Course>();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.Get_AllCoursesANDCourseCodes", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    List<Prerequisite> prereqs = PrerequisiteDAL.GetPrereqs(Convert.ToInt32(reader["courseId"]));
                    Course c = CourseFromRow(reader, prereqs);
                    courses.Add(c);
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
            return courses;//return the list of courses
        }
        public static Course GetCourseById(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            Course course = null;
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectCourseByid", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    List<Prerequisite> prereqs = PrerequisiteDAL.GetPrereqs(Convert.ToInt32(reader["courseId"]));
                    course = CourseFromRow(reader, prereqs);
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
            return course;//return the course
        }

        public static bool UpdateCourse(Course c)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            try
            {
                conn.Open();
                SqlCommand updateCommand = new SqlCommand("mcftacademics.dbo.UpdateCourseById", conn);
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
                    PrerequisiteDAL.DropPrereqs(c.Id);
                    foreach (Prerequisite prereq in c.Prerequisites)
                    {
                        PrerequisiteDAL.AddPrereq(prereq);
                    }
                    result= true;
                }
                else
                {
                    result= false;
                }
            }
            catch(Exception ex)
            {
                result = false;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public static int AddCourse(Course c)
        {
            SqlConnection conn = DbConn.GetConnection();
            int id;
            try
            {
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertCourse", conn);
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
                    SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectLastCourseInsert", conn);
                    id = Convert.ToInt32(selectCommand.ExecuteScalar());
                    PrerequisiteDAL.DropPrereqs(id);
                    foreach (Prerequisite prereq in c.Prerequisites)
                    {
                        prereq.CourseId = id;
                        PrerequisiteDAL.AddPrereq(prereq);
                    }
                }
                else
                {
                    throw new Exception();
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
            return id;
        }
    }
}
