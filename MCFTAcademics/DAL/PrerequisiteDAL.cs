using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public class PrerequisiteDAL
    {
        public static List<Prerequisite> getPrereqs(int id)
        {
            SqlConnection conn = DbConn.GetConnection();
            conn.Open(); //open the connection
            SqlCommand selectCommand = new SqlCommand("mcftacademics.dbo.SelectPrereqsById", conn);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@id", id);
            //execute the sql statement
            SqlDataReader reader = selectCommand.ExecuteReader();
            List<Prerequisite> prereqs = new List<Prerequisite>();
            //loop through the resultset
            while (reader.Read())
            {
                Prerequisite prereq = new Prerequisite(Convert.ToInt32(reader["courseId"]), Convert.ToInt32(reader["prereqId"]), Convert.ToBoolean(reader["isPrereq"]), Convert.ToBoolean(reader["isCoreq"]));
                prereqs.Add(prereq);
            }
            conn.Close();//don't forget to close the connection
            return prereqs;//return the list of prereqs
        }
    }
}
