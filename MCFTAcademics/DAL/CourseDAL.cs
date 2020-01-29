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
                Course c = new Course(Convert.ToInt32(reader["courseId"]), reader["name"].ToString(), Convert.ToDecimal(reader["credit"]), DateTime.Parse(reader["startDate"].ToString()),DateTime.Parse(reader["endDate"].ToString()),reader["Description"].ToString(),Convert.ToInt32(reader["lectureHours"]),Convert.ToInt32(reader["labHours"]),Convert.ToInt32(reader["examHours"]),Convert.ToInt32(reader["totalHours"]), prereqs);
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
                course = new Course(Convert.ToInt32(reader["courseId"]), reader["name"].ToString(), Convert.ToDecimal(reader["credit"]), DateTime.Parse(reader["startDate"].ToString()), DateTime.Parse(reader["endDate"].ToString()), reader["Description"].ToString(), Convert.ToInt32(reader["lectureHours"]), Convert.ToInt32(reader["labHours"]), Convert.ToInt32(reader["examHours"]), Convert.ToInt32(reader["totalHours"]), prereqs);
            }
            conn.Close();//don't forget to close the connection
            return course;//return the course
        }
    }
}
