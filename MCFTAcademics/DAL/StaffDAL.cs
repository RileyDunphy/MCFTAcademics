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

        public static Staff GetStaffByCourseIdAndType(int id, string type)
        {
            Staff staff = null;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStaffByCourseIdAndType", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@id", id);
                selectCommand.Parameters.AddWithValue("@type", type);
                //execute the sql statement
                SqlDataReader reader = selectCommand.ExecuteReader();
                //loop through the resultset
                if (reader.Read())
                {
                    staff = StaffFromRow(reader);
                }
            }
            return staff;//return the staff
        }
        public static bool AddStaff(int courseId, Staff staff)
        {
            bool result;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertStaff", connection);
                insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                insertCommand.Parameters.AddWithValue("@courseId", courseId);
                insertCommand.Parameters.AddWithValue("@staffId", staff.UserId);
                insertCommand.Parameters.AddWithValue("@type", staff.Type);
                int rows = insertCommand.ExecuteNonQuery();
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

        public static bool DropStaff(int id)
        {
            bool result;
            using (var connection = DbConn.GetConnection())
            {
                connection.Open();
                SqlCommand deleteCommand = new SqlCommand("mcftacademics.dbo.DropAllStaffById", connection);
                deleteCommand.CommandType = System.Data.CommandType.StoredProcedure;
                deleteCommand.Parameters.AddWithValue("@courseId", id);
                int rows = deleteCommand.ExecuteNonQuery();
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
    }
}
