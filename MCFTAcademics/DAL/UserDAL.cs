using MCFTAcademics.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public static class UserDAL
    {
        static User UserFromRow(IDataReader reader)
        {
            var id = (int)reader["userId"];
            var name = (string)reader["username"];
            var password = (string)reader["password"];
            var realname = (string)reader["realName"];
            return new User(realname, name, password, id);
        }

        public static User GetUser(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = DbConn.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "mcftacademics.dbo.Get_User_ById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userIdentity", id);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                    return null;
                return UserFromRow(reader);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static User GetUser(string username)
        {
            SqlConnection connection = null;
            try
            {
                connection = DbConn.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "mcftacademics.dbo.Get_User_ByName";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", username);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                    return null;
                return UserFromRow(reader);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
}
