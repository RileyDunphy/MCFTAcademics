﻿using Flee.PublicTypes;
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
        static Grade GradeFromRow(IDataReader reader, Course course = null)
        {
            var studentId = (int)reader["studentId"];
            var locked = (bool)reader["lock"];
            var supplemental = (bool)reader["isSupplemental"];
            var given = (DateTime)reader["given"];
            var hoursAttended = (decimal)reader["hoursAttended"];
            var grade = (decimal)reader["grade"];
            var comment = "";
            if (reader["comment"] != DBNull.Value)
            {
                comment = (string)reader["comment"];
            }
            if (course == null)
            {
                course = CourseDAL.GetCourseById((int)reader["courseId"]);
            }
            // XXX: Preserve the staff stuff too?
            return new Grade(studentId,grade, given, locked, hoursAttended, supplemental, course,comment);
        }

        public static IEnumerable<Grade> GetAllGrades()
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "mcftacademics.dbo.Get_All_Grades";
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                    yield return GradeFromRow(reader);
            }
        }

        // XXX: This is probably very wrong. CourseStaff is halfway a
        // table for m:n relations, but can also represent temps... which
        // the schema can't allow for. As such, instructors can only be
        // represented by User since it's the only other PK usable, so...
        // (This should really be changed.)
        public static IEnumerable<Grade> GetGradesForInstructor(User staff)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "mcftacademics.dbo.Get_Grades_ByStaff";
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                    yield return GradeFromRow(reader);
            }
        }

        // moved from StudentDAL
        internal static List<Grade> GetGradesForStudent(Student student)
        {
            List<Grade> grades = new List<Grade>();
            Grade grade = null;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentGradeById2", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@studentId", student.Id);
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    grade = GradeDAL.GradeFromRow(reader);
                    grades.Add(grade);
                }
            }
            return grades;
        }

        internal static Grade GetGradesForStudentInCourse(Course course, Student student)
        {
            Grade grade = null;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectGradeByCourseAndStudent", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@courseId", course.Id);
                selectCommand.Parameters.AddWithValue("@studentId", student.Id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    grade = GradeDAL.GradeFromRow(reader, course);
                }
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

        public static bool UpdateGrade(Grade grade, int studentId)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            try
            {
                conn.Open();
                SqlCommand updateCommand = new SqlCommand("mcftacademics.dbo.UpdateGradeByStudentIdAndCourseId", conn);
                updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
                updateCommand.Parameters.AddWithValue("@studentId", studentId);
                updateCommand.Parameters.AddWithValue("@courseId", grade.Subject.Id);
                updateCommand.Parameters.AddWithValue("@grade", grade.GradeAssigned);
                updateCommand.Parameters.AddWithValue("@comment", grade.Comment);
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
        internal static List<Grade> GetGradesForStudentSemester(Student student, int semester)
        {
            List<Grade> grades = new List<Grade>();
            Grade grade = null;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStudentGradeByIdAndSemester", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", student.Id);
                selectCommand.Parameters.AddWithValue("@semester", semester);
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    grade = GradeDAL.GradeFromRow(reader);
                    grades.Add(grade);
                }
            }
            return grades;
        }

        internal static decimal GetAverageForStudentSemester(Student student, int semester)
        {//Old way of doing it, probably use the formula building way instead of this now
            decimal average=0;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectAverageByIdAndSemester", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", student.Id);
                selectCommand.Parameters.AddWithValue("@semester", semester);
                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["average"]==DBNull.Value)
                    {
                        average = 0;
                    }
                    else
                    {
                        average = (decimal)reader["average"];
                    }
                }
            }
            return Math.Round(average,2);
        }
        internal static decimal GetAverageForStudentOLD(Student student)
        {//Old way of doing it, probably use the formula building way instead of this now
            decimal average = 0;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectAverageById", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", student.Id);
                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["average"] == DBNull.Value)
                    {
                        average = 0;
                    }
                    else
                    {
                        average = (decimal)reader["average"];
                    }
                }
            }
            return Math.Round(average, 2);
        }
        public static Grade GetSummerPracticum(Student s)
        {
            Grade grade = null;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectSummerPracticumByStudentId", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@studentId", s.Id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    grade = GradeDAL.GradeFromRow(reader);
                }
            }
            return grade;//return the grade
        }

        public static bool UpdateFormula(string formula)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            using (var connection = DbConn.GetConnection())
            {
                conn.Open();
                SqlCommand updateCommand = new SqlCommand("mcftacademics.dbo.UpdateFormula", conn);
                updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
                updateCommand.Parameters.AddWithValue("@formula", formula);
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
            return result;
        }

        internal static decimal GetAverageForStudent(Student student, int semester = -1)
        {
            //Get all Grades for the Student, by program if no semester passed in, or by semester if it is
            IEnumerable<Grade> grades = semester != -1 ? student.GetGradesForSemester(semester) : student.GetGrades();
            List<decimal> results = new List<decimal>();
            decimal average = 0;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectFormula", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.Read())
                {
                    // Define the context of our expression
                    ExpressionContext context = new ExpressionContext();
                    // Allow the expression to use all static public methods of System.Math
                    context.Imports.AddType(typeof(Math));
                    //Get the total Course credit hours for use in the expression
                    context.Variables["c"] = grades.Sum(gg => gg.Subject.Credit);
                    foreach (Grade g in grades)
                    {
                        // Define an int variable
                        if (g.Supplemental)
                        {
                            context.Variables["a"] = 60m;
                        }
                        else
                        {
                            context.Variables["a"] = g.GradeAssigned;
                        }
                        context.Variables["b"] = g.Subject.Credit;

                        // Create a dynamic expression that evaluates to an Object
                        IDynamicExpression eDynamic = context.CompileDynamic(reader["Formula"].ToString());

                        // Evaluate the expressions
                        decimal result = (decimal)eDynamic.Evaluate();
                        results.Add(result);
                    }
                    average = results.Sum();
                }
            }
            return Math.Round(average, 2);
        }
    }
}
