﻿using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MCFTAcademics.DAL
{
    public class StudentDAL
    {
        private static Student StudentFromRow(IDataReader reader)
        {
            var id = Convert.ToInt32(reader["studentId"]);
            var academicAccommodation = Convert.ToBoolean(reader["academicAccommodation"]);
            var firstName = reader["firstName"].ToString();
            var lastName = reader["lastName"].ToString();
            var studentCode = reader["studentCode"].ToString();
            var program = reader["program"].ToString();
            var admissionDate = reader["admissionDate"] is DateTime ? (DateTime?)reader["admissionDate"] : null;
            var graduationDate = reader["graduationDate"] is DateTime ? (DateTime?)reader["graduationDate"] : null;
            return new Student(id, firstName, lastName, studentCode, program, admissionDate,graduationDate, academicAccommodation);
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
        public static Student GetStudentByStudentId(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentsByStudentId", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@studentId", id);
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
            return null;//if no student was found
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
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.[SelectStudentsByStudentCode]", connection);
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
            List<Student> students = new List<Student>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectAllStudents", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Student s = StudentFromRow(reader);
                    students.Add(s);
                }
                return students;//return the list of students
            }
            return null;
        }

        public static string GetClassRank(Student s)
        {
            SortedList<int, decimal> students = new SortedList<int, decimal>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectAllStudentsByClass", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@program", s.Program);
                selectCommand.Parameters.AddWithValue("@graduationDate", s.GraduationDate);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Student student = StudentFromRow(reader);
                    students.Add(student.Id, student.GetAverage());
                }
                int rank = students.IndexOfKey(s.Id) + 1;
                int total = students.Count;
                return rank + " of " + total;
            }
            return null;
        }

        public static int AddStudent(Student s)
        {
            int id;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction("AddStudent");
                try
                {
                    SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertAndEnrollStudent", connection);
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    insertCommand.Parameters.AddWithValue("@firstName", s.FirstName);
                    insertCommand.Parameters.AddWithValue("@lastName", s.LastName);
                    insertCommand.Parameters.AddWithValue("@studentCode", s.StudentCode);
                    insertCommand.Parameters.AddWithValue("@program", s.Program);
                    insertCommand.Parameters.AddWithValue("@gradDate", s.GraduationDate);
                    insertCommand.Parameters.AddWithValue("@academicAccommodation", s.AcademicAccommodation);
                    var reader = insertCommand.ExecuteReader();
                    if (!reader.Read())
                        return 0;
                    id = Convert.ToInt32(reader["id"]);
                    reader.Close();
                    transaction.Commit();
                    return id;
                }
                catch (Exception ex)
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