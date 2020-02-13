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

        public static List<Prerequisite> GetPrereqs(SqlConnection connection, int id)
        {
            List<Prerequisite> prereqs = new List<Prerequisite>();
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
            return prereqs;
        }

        public static List<Prerequisite> GetPrereqs(int id)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                return GetPrereqs(connection, id);
            }
        }

        public static bool DropPrereqs(SqlConnection connection, int id)
        {
            SqlCommand deleteCommand = new SqlCommand("mcftacademics.dbo.DropAllPrereqsById", connection);
            deleteCommand.CommandType = System.Data.CommandType.StoredProcedure;
            deleteCommand.Parameters.AddWithValue("@id", id);
            int rows = deleteCommand.ExecuteNonQuery();
            return (rows > 0);
        }

        public static bool DropPrereqs(int id)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                return DropPrereqs(connection, id);
            }
        }

        public static bool AddPrereq(SqlConnection connection, Prerequisite prereq)
        {
            SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertPrereq", connection);
            insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
            insertCommand.Parameters.AddWithValue("@courseId", prereq.CourseId);
            insertCommand.Parameters.AddWithValue("@prereqId", prereq.PrereqId);
            insertCommand.Parameters.AddWithValue("@isPrereq", prereq.IsPrereq);
            insertCommand.Parameters.AddWithValue("@isCoreq", prereq.IsCoreq);
            int rows = insertCommand.ExecuteNonQuery();
            return (rows > 0);
        }

        public static bool AddPrereq(Prerequisite prereq)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                return AddPrereq(connection, prereq);
            }
        }
    }
}