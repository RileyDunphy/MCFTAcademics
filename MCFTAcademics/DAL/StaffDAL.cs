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
            SqlConnection conn = DbConn.GetConnection();
            List<Staff> staff = new List<Staff>();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.GetAllInstructors", conn);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();//don't forget to close the connection
            }
            return staff;//return the list of staff
        }

        public static Staff GetStaffByCourseIdAndType(int id, string type)
        {
            Staff staff = null;
            SqlConnection conn = DbConn.GetConnection();
            try
            {
                conn.Open(); //open the connection
                SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectStaffByCourseIdAndType", conn);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();//don't forget to close the connection
            }
            return staff;//return the staff
        }
        public static bool AddStaff(int courseId, Staff staff)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            try
            {
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertStaff", conn);
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

        public static bool DropStaff(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            try
            {
                conn.Open(); //open the connection
                SqlCommand deleteCommand = new SqlCommand("mcftacademics.dbo.DropAllStaffById", conn);
                deleteCommand.CommandType = System.Data.CommandType.StoredProcedure;
                deleteCommand.Parameters.AddWithValue("@id", id);
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
    }
}
