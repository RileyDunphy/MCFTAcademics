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
                Course c = new Course(reader["name"].ToString(), Convert.ToDecimal(reader["credit"]), DateTime.Parse(reader["startDate"].ToString()),DateTime.Parse(reader["endDate"].ToString()));
                courses.Add(c);
            }
            conn.Close();//don't forget to close the connection
            return courses;//return the list of courses
        }
    }
}
