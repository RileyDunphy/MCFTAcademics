﻿using MCFTAcademics.BL;
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
        static Course CourseFromRow(IDataReader reader, List<Prerequisite> prereqs, Staff leadStaff, Staff supportStaff)
        {
            return new Course(Convert.ToInt32(reader["courseId"]), reader["name"].ToString(), Convert.ToDecimal(reader["credit"]), reader["Description"].ToString(), Convert.ToInt32(reader["lectureHours"]), Convert.ToInt32(reader["labHours"]), Convert.ToInt32(reader["examHours"]), Convert.ToInt32(reader["totalHours"]), Convert.ToDecimal(reader["revisionNumber"]), reader["program"].ToString(), Convert.ToBoolean(reader["accreditation"]), prereqs, leadStaff, supportStaff);
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
                    Staff leadStaff = StaffDAL.GetStaffByCourseIdAndType(Convert.ToInt32(reader["courseId"]),"lead");
                    Staff supportStaff = StaffDAL.GetStaffByCourseIdAndType(Convert.ToInt32(reader["courseId"]),"support");
                    List<Prerequisite> prereqs = PrerequisiteDAL.GetPrereqs(Convert.ToInt32(reader["courseId"]));
                    Course c = CourseFromRow(reader, prereqs, leadStaff, supportStaff);
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
                    Course c = CourseFromRow(reader, null,null,null);
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
                    Staff leadStaff = StaffDAL.GetStaffByCourseIdAndType(Convert.ToInt32(reader["courseId"]), "lead");
                    Staff supportStaff = StaffDAL.GetStaffByCourseIdAndType(Convert.ToInt32(reader["courseId"]), "support");
                    List<Prerequisite> prereqs = PrerequisiteDAL.GetPrereqs(Convert.ToInt32(reader["courseId"]));
                    course = CourseFromRow(reader, prereqs, leadStaff, supportStaff);
                }
            }
            return course;//return the course
        }

        public static bool UpdateCourse(Course c)
        {
            bool result;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand updateCommand = new SqlCommand("mcftacademics.dbo.UpdateCourseById", connection);
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
                    StaffDAL.DropStaff(c.Id);
                    StaffDAL.AddStaff(c.Id, c.LeadStaff);
                    if (c.SupportStaff != null)
                    {
                        StaffDAL.AddStaff(c.Id, c.SupportStaff);
                    }
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
            return result;
        }

        public static int AddCourse(Course c)
        {
            int id;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertCourse", connection);
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
                    id = Convert.ToInt32(selectCommand.ExecuteScalar());
                    //Drop existing staff (so they're not multiple instructors) 
                    //and add back the lead staff and support if there is one
                    StaffDAL.DropStaff(id);
                    StaffDAL.AddStaff(id, c.LeadStaff);
                    if(c.SupportStaff != null)
                    {
                        StaffDAL.AddStaff(id, c.SupportStaff);
                    }
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
            return id;
        }
    }
}
