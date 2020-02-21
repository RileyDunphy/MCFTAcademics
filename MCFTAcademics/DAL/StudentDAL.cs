﻿using MCFTAcademics.BL;
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
            var id = Convert.ToInt32(reader["studentId"]);
            var firstName = reader["firstName"].ToString();
            var lastName = reader["lastName"].ToString();
            var studentCode = reader["studentCode"].ToString();
            var program = reader["program"].ToString();
            var admissionDate = reader["admissionDate"] is DateTime ? (DateTime?)reader["admissionDate"] : null;
            var graduationDate = reader["graduationDate"] is DateTime ? (DateTime?)reader["graduationDate"] : null;
            return new Student(id, firstName, lastName, studentCode, program, admissionDate,graduationDate);
        } 

        public static List<Student> GetStudentsInCourse(Course course)
        {
            List<Student> students = new List<Student>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentsByCourseId", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@courseId", course.Id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Student s = StudentFromRow(reader);
                    students.Add(s);
                }
            }
            return students;//return the list of students
        }

        public static Student GetStudent(int id)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.[SelectStudentsByStudentId]", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@studentId", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    return StudentFromRow(reader);
                }
            }
            return null;
        }

        public static Student GetStudent(string code)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.[SelectStudentsByStudentId]", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@studentCode", code);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Student s = StudentFromRow(reader);
                    return s;
                }
            }
            return null;
        }

        public static List<Student> GetAllStudents()
        {
            SqlConnection conn = DbConn.GetConnection();
            List<Student> students = new List<Student>();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectAllStudents", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
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
                conn.Close();//don't forget to close the connection
            }
            return students;//return the list of students
        }

        public static string GetClassRank(Student s)
        {
            SortedList<int, decimal> ranks = new SortedList<int, decimal>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.[GetClassRanks]", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@program", s.Program);
                selectCommand.Parameters.AddWithValue("@graduationDate", s.GraduationDate);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    ranks.Add(Convert.ToInt32(reader["id"]), Convert.ToDecimal(reader["average"]));
                }
                int rank = ranks.IndexOfKey(s.Id) + 1;
                int total = ranks.Count;
                return rank + " of " + total;
            }
            return null;
        }

    }
}