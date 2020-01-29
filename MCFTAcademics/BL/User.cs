using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class User
    {
        internal User(string name, string username, string password, int id)
        {
            this.Name = name;
            this.Username = username;
            this.Password = password;
        }

        /// <summary>
        /// Verifies if a password given matches the hashed password stored in the user.
        /// </summary>
        /// <param name="password">The password provided to check against the hash.</param>
        /// <returns>If they compare against the same value.</returns>
        public bool ValidatePassword(string password) => BCrypt.Net.BCrypt.Verify(password, this.Password);

        // it IMHO makes more sense here than in Role (since it's from the user)

        // XXX: Cache these/get on acquiring the user object, instead of constantly refetching
        // We only get them at login time for now though, since we convert them into Claims
        public IEnumerable<Role> GetRoles() => RoleDAL.GetRolesForUser(this);

        public static User GetUser(int id) => UserDAL.GetUser(id);

        public static User GetUser(string username) => UserDAL.GetUser(username);

        public string Name { get;}
        public string Username { get; }
        public string Password { get;  }
        public int Id { get; }
    }
}
