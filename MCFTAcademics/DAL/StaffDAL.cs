using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class StaffDAL
    {
        static Staff StaffFromRow(IDataReader reader)
        {
            return new Staff(Convert.ToInt32(reader["userId"]),reader["realName"].ToString(), reader["instructorType"].ToString());
        }
        public static List<Staff> GetAllStaff()
        {
            List<Staff> staff = new List<Staff>();
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.GetAllInstructors", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                while (reader.Read())
                {
                    Staff c = StaffFromRow(reader);
                    staff.Add(c);
                }
            }
            return staff;//return the list of staff
        }

        internal static Staff GetStaffByCourseIdAndType(SqlConnection connection, int id, string type)
        {
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStaffByCourseIdAndType", connection);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@id", id);
            selectCommand.Parameters.AddWithValue("@type", type);
            //execute the sql statement
            SqlDataReader reader = selectCommand.ExecuteReader();
            //loop through the resultset
            if (reader.Read())
            {
                return StaffFromRow(reader);
            }
            return null;
        }

        public static Staff GetStaffByCourseIdAndType(int id, string type)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                return GetStaffByCourseIdAndType(connection, id, type);
            }
        }

        internal static bool AddStaff(SqlConnection connection, int courseId, Staff staff, SqlTransaction transaction = null)
        {
            SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertStaff", connection);
            if (transaction != null)
                insertCommand.Transaction = transaction;
            insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
            insertCommand.Parameters.AddWithValue("@courseId", courseId);
            insertCommand.Parameters.AddWithValue("@staffId", staff.UserId);
            insertCommand.Parameters.AddWithValue("@type", staff.Type);
            int rows = insertCommand.ExecuteNonQuery();
            return (rows > 0);
        }

        public static bool AddStaff(int courseId, Staff staff)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                return AddStaff(connection, courseId, staff);
            }
        }

        public static bool DropStaff(SqlConnection connection, int id, SqlTransaction transaction = null)
        {
            SqlCommand deleteCommand = new SqlCommand("mcftacademics.dbo.DropAllStaffById", connection);
            if (transaction != null)
                deleteCommand.Transaction = transaction;
            deleteCommand.CommandType = System.Data.CommandType.StoredProcedure;
            deleteCommand.Parameters.AddWithValue("@courseId", id);
            int rows = deleteCommand.ExecuteNonQuery();
            return (rows > 0);
        }

        public static bool DropStaff(int id)
        {
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                return DropStaff(connection, id);
            }
        }
    }
}
