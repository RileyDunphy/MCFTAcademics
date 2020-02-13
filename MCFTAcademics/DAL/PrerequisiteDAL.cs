using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MCFTAcademics.DAL
{
    public class PrerequisiteDAL
    {
        private static Prerequisite PrerequisiteFromRow(IDataReader reader)
        {
            return new Prerequisite(Convert.ToInt32(reader["courseId"]), Convert.ToInt32(reader["prereqId"]), Convert.ToBoolean(reader["isPrereq"]), Convert.ToBoolean(reader["isCoreq"]));
        }

        public static List<Prerequisite> GetPrereqs(int id)
        {
            List<Prerequisite> prereqs = new List<Prerequisite>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectPrereqsById", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", id);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    prereqs.Add(PrerequisiteFromRow(reader));
                }
            }
            return prereqs;//return the list of prereqs
        }

        public static bool DropPrereqs(int id)
        {
            bool result;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand deleteCommand = new SqlCommand("mcftacademics.dbo.DropAllPrereqsById", connection);
                deleteCommand.CommandType = System.Data.CommandType.StoredProcedure;
                deleteCommand.Parameters.AddWithValue("@id", id);
                int rows = deleteCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    result= true;
                }
                else
                {
                    result= false;
                }
            }
            return result;
        }

        public static bool AddPrereq(Prerequisite prereq)
        {
            bool result;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertPrereq", connection);
                insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                insertCommand.Parameters.AddWithValue("@courseId", prereq.CourseId);
                insertCommand.Parameters.AddWithValue("@prereqId", prereq.PrereqId);
                insertCommand.Parameters.AddWithValue("@isPrereq", prereq.IsPrereq);
                insertCommand.Parameters.AddWithValue("@isCoreq", prereq.IsCoreq);
                int rows = insertCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    result= true;
                }
                else
                {
                    result= false;
                }
            }
            return result;
        }
    }
}