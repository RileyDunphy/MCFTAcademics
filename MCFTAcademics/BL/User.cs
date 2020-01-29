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

        public static User GetUser(int id) => UserDAL.GetUser(id);

        public static User GetUser(string username) => UserDAL.GetUser(username);

        public string Name { get;}
        public string Username { get; }
        public string Password { get;  }
        public int Id { get; }
    }
}
