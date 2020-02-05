﻿using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class CourseDAL
    {
        public static List<Course> getAllCourses()
        {
            SqlConnection conn = DbConn.GetConnection();
            conn.Open(); //open the connection
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.Get_AllCoursesANDCourseCodes", conn);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //execute the sql statement
            SqlDataReader reader = selectCommand.ExecuteReader();
            List<Course> courses = new List<Course>();
            //loop through the resultset
            while (reader.Read())
            {
                List<Prerequisite> prereqs = PrerequisiteDAL.getPrereqs(Convert.ToInt32(reader["courseId"]));
                Course c = new Course(Convert.ToInt32(reader["courseId"]), reader["name"].ToString(), Convert.ToDecimal(reader["credit"]), DateTime.Parse(reader["startDate"].ToString()),DateTime.Parse(reader["endDate"].ToString()),reader["Description"].ToString(),Convert.ToInt32(reader["lectureHours"]),Convert.ToInt32(reader["labHours"]),Convert.ToInt32(reader["examHours"]),Convert.ToInt32(reader["totalHours"]), Convert.ToDecimal(reader["revisionNumber"]), prereqs);
                courses.Add(c);
            }
            conn.Close();//don't forget to close the connection
            return courses;//return the list of courses
        }
        public static Course getCourseById(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            conn.Open(); //open the connection
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectCourseByid", conn);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@id", id);
            //execute the sql statement
            SqlDataReader reader = selectCommand.ExecuteReader();
            Course course = new Course();
            //loop through the resultset
            while (reader.Read())
            {
                List<Prerequisite> prereqs = PrerequisiteDAL.getPrereqs(Convert.ToInt32(reader["courseId"]));
                course = new Course(Convert.ToInt32(reader["courseId"]), reader["name"].ToString(), Convert.ToDecimal(reader["credit"]), DateTime.Parse(reader["startDate"].ToString()), DateTime.Parse(reader["endDate"].ToString()), reader["Description"].ToString(), Convert.ToInt32(reader["lectureHours"]), Convert.ToInt32(reader["labHours"]), Convert.ToInt32(reader["examHours"]), Convert.ToInt32(reader["totalHours"]), Convert.ToDecimal(reader["revisionNumber"]), prereqs);
            }
            conn.Close();//don't forget to close the connection
            return course;//return the course
        }

        public static bool updateCourse(Course c)
        {
            SqlConnection conn = DbConn.GetConnection();
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
            int rows = updateCommand.ExecuteNonQuery();
            if (rows > 0)
            {
                PrerequisiteDAL.dropPrereqs(c.Id);
                foreach (Prerequisite prereq in c.Prerequisites)
                {
                    PrerequisiteDAL.addPrereq(prereq);
                }
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }

        public static int addCourse(Course c)
        {
            SqlConnection conn = DbConn.GetConnection();
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
            int rows = insertCommand.ExecuteNonQuery();
            if (rows > 0)
            {
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectLastCourseInsert", conn);
                int id = Convert.ToInt32(selectCommand.ExecuteScalar());
                PrerequisiteDAL.dropPrereqs(id);
                foreach (Prerequisite prereq in c.Prerequisites)
                {
                    prereq.CourseId = id;
                    PrerequisiteDAL.addPrereq(prereq);
                }
                conn.Close();
                return id;
            }
            else
            {
                conn.Close();
                throw new Exception();
            }
        }
    }
}
