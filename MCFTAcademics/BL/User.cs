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
            this.Id = id;
        }

        /// <summary>
        /// Verifies if a password given matches the hashed password stored in the user.
        /// </summary>
        /// <param name="password">The password provided to check against the hash.</param>
        /// <returns>If they compare against the same value.</returns>
        public bool ValidatePassword(string password) => BCrypt.Net.BCrypt.Verify(password, this.Password);

        // This function actually provides an example of why the classes here are immutable.
        // No objects change from under you, and you can replace the previous object with
        // the new one as necessary.
        /// <summary>
        /// Changes the user's password in the database and provides a replacement user object.
        /// </summary>
        /// <param name="newPasswordUnhashed">The new password before hashing.</param>
        /// <returns>The user object with a new password, or null if unable to.</returns>
        public User ChangePassword(string newPasswordUnhashed)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(newPasswordUnhashed);
            if (UserDAL.ChangePassword(this, hashed))
            {
                return new User(this.Name, this.Username, hashed, this.Id);
            }
            return null;
        }

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
