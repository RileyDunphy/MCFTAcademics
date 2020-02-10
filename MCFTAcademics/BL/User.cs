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
        public bool ValidatePassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            // a password of just spaces is technically valid?
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));

            return BCrypt.Net.BCrypt.Verify(password, this.Password);
        }

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
            if (newPasswordUnhashed == null)
                throw new ArgumentNullException(nameof(newPasswordUnhashed));
            // a password of just spaces is technically valid?
            if (string.IsNullOrEmpty(newPasswordUnhashed))
                throw new ArgumentException(nameof(newPasswordUnhashed));
            var hashed = BCrypt.Net.BCrypt.HashPassword(newPasswordUnhashed);
            if (UserDAL.ChangePassword(this, hashed))
            {
                return new User(this.Name, this.Username, hashed, this.Id);
            }
            return null;
        }

        /// <summary>
        /// Blanks out a user's password to prevent them from logging in.
        /// </summary>
        /// <returns>The user after having the change applied.</returns>
        public User DisableUser()
        {
            if (UserDAL.ChangePassword(this, ""))
            {
                return new User(this.Name, this.Username, "", this.Id);
            }
            return null;
        }

        /// <summary>
        /// Gets if a user is disabled (as in their account) to prevent login.
        /// </summary>
        public bool IsDisabled => string.IsNullOrWhiteSpace(Password);

        /// <summary>
        /// Changes the user's profile fields.
        /// </summary>
        /// <param name="newRealName">The new real name of the user.</param>
        /// <param name="newUserName">The new user name of the user.</param>
        /// <returns>The user with a new profile, or null if unable to.</returns>
        public User ChangeProfile(string newRealName, string newUserName)
        {
            if (newRealName == null)
                throw new ArgumentNullException(nameof(newRealName));
            if (string.IsNullOrWhiteSpace(newRealName))
                throw new ArgumentException(nameof(newRealName));
            if (newUserName == null)
                throw new ArgumentNullException(nameof(newUserName));
            if (string.IsNullOrWhiteSpace(newUserName))
                throw new ArgumentException(nameof(newUserName));
            // XXX: More validation
            if (UserDAL.ChangeProfile(this, newRealName, newUserName))
            {
                return new User(newRealName, newUserName, this.Password, this.Id);
            }
            return null;
        }

        // it IMHO makes more sense here than in Role (since it's from the user)

        // XXX: Cache these/get on acquiring the user object, instead of constantly refetching
        // We only get them at login time for now though, since we convert them into Claims
        public IEnumerable<Role> GetRoles() => RoleDAL.GetRolesForUser(this);

        public Role GetRole(int roleId) => RoleDAL.GetRole(this, roleId);

        public static IEnumerable<User> GetAllUsers() => UserDAL.GetAllUsers();

        public static User GetUser(int id) => UserDAL.GetUser(id);

        public static User CreateUser(string name, string username, string password)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            // a password of just spaces is technically valid?
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));

            // hash the pw since we have it unhashed
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User(name, username, hashedPassword, -1);
            // same but with the new ID and such
            var insertedUser = UserDAL.CreateUser(newUser);
            return insertedUser;
        }

        public static User GetUser(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(nameof(username));

            return UserDAL.GetUser(username);
        }

        public string Name { get;}
        public string Username { get; }
        public string Password { get;  }
        // XXX: What is the valid range of UIDs?
        public int Id { get; }
    }
}
