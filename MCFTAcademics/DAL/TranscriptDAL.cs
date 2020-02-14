using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class TranscriptDAL
    {
        public static bool AddTranscript(string path, Student student)
        {
            SqlConnection conn = DbConn.GetConnection();
            bool result;
            try
            {
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("mcftacademics.dbo.InsertTranscript", conn);
                insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                insertCommand.Parameters.AddWithValue("@studentId", student.Id);
                insertCommand.Parameters.AddWithValue("@path", path);
                int rows = insertCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    result= false;
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
