﻿using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class CourseCodeDAL
    {
        public static CourseCode getNewestCourseCodeById(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            conn.Open(); //open the connection
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectNewestCourseCodeById", conn);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@id", id);
            //execute the sql statement
            SqlDataReader reader = selectCommand.ExecuteReader();
            CourseCode courseCode = new CourseCode();
            //loop through the resultset
            while (reader.Read())
            {
                courseCode = new CourseCode(reader["courseCode"].ToString(), DateTime.Parse(reader["startDate"].ToString()), DateTime.Parse(reader["endDate"].ToString()), Convert.ToDecimal(reader["revisionNumber"]));
            }
            conn.Close();//don't forget to close the connection
            return courseCode;//return the course
        }
    }
}
