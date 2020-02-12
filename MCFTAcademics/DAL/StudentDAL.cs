using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MCFTAcademics.DAL
{
    public class StudentDAL
    {
        private static Student StudentFromRow(IDataReader reader)
        {
            List<Grade> g =GetGradeByStudentId((int)reader["studentId"]);
            return new Student(g, reader["firstName"].ToString(), reader["lastName"].ToString(), reader["studentId"].ToString(), DateTime.Now);
        } 

        public static List<Student> GetStudentsByCourseId(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            List<Student> students = new List<Student>();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentsByCourseId", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@courseId", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Student s = StudentFromRow(reader);
                    students.Add(s);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return students;//return the list of students
        }
        public static Student GetStudentByStudentId(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentsByCourseId", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@courseId", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Student s = StudentFromRow(reader);
                    return s;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return null;//return the list of students
        }

        internal static List<Grade> GetGradeByStudentId(int studentId)
        {
            SqlConnection conn = DbConn.GetConnection();
            List<Grade> grades = new List<Grade>();
            Grade grade = null;
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentGradeById2", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@studentId", studentId);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
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


        internal static Grade GetGradeByCourseId(int id, Student student)
        {
            SqlConnection conn = DbConn.GetConnection();
            Grade grade = null;
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectGradeByCourseAndStudent", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@courseId", id);
                selectCommand.Parameters.AddWithValue("@studentId", student.StudentId);
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
    }
}