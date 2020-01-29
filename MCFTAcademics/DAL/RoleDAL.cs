using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public static class RoleDAL
    {
        static Role RoleFromRow(IDataReader reader, User user)
        {
            var name = (string)reader["roleName"];
            var id = (int)reader["roleId"];
            // we have the user already, no need to refetch
            return new Role(name, id, user);
        }

        public static IEnumerable<Role> GetRolesForUser(User user)
        {
            var roles = new List<Role>();
            SqlConnection connection = null;
            try
            {
                connection = DbConn.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                // without _ById, this accepts Username instead
                command.CommandText = "mcftacademics.dbo.Get_UserRoles_ById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userIdentity", user.Id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                    roles.Add(RoleFromRow(reader, user));
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return roles;
        }
    }
}
