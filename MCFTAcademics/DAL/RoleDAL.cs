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
            var id = Convert.ToInt32(reader["roleId"]);
            // we have the user already, no need to refetch
            return new Role(name, id, user);
        }

        public static Role Grant(Role role)
        {
            SqlConnection connection = null;
            try
            {
                connection = DAL.DbConn.GetConnection();
                connection.Open();
                var sql = "[mcftacademics].dbo.[Grant_UserRole]";
                var query = connection.CreateCommand();
                query.CommandType = CommandType.StoredProcedure;
                query.CommandText = sql;
                query.Parameters.AddWithValue("@userIdentity", role.User.Id);
                query.Parameters.AddWithValue("@roleName", role.Name);
                // depends on set nocount off being in the procedure
                var reader = query.ExecuteReader();
                if (!reader.Read())
                    return null;
                return RoleFromRow(reader, role.User);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool Revoke(Role role)
        {
            SqlConnection connection = null;
            try
            {
                connection = DAL.DbConn.GetConnection();
                connection.Open();
                var sql = "[mcftacademics].dbo.[Revoke_UserRole_ById]";
                var query = connection.CreateCommand();
                query.CommandType = CommandType.StoredProcedure;
                query.CommandText = sql;
                query.Parameters.AddWithValue("@userIdentity", role.User.Id);
                query.Parameters.AddWithValue("@roleIdentity", role.Id);
                // depends on set nocount off being in the procedure
                return (query.ExecuteNonQuery() > 0);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static Role GetRole(User user, int roleId)
        {
            SqlConnection connection = null;
            try
            {
                connection = DbConn.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "mcftacademics.dbo.Get_UserRole_ById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userIdentity", user.Id);
                command.Parameters.AddWithValue("@roleIdentity", roleId);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                    return null;
                return RoleFromRow(reader, user);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
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
