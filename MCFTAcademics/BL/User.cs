using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class User
    {
        private string name;
        private string username;
        private string password;

        public User()
        {
        }

        public User(string name, string username, string password)
        {
            this.Name = name;
            this.Username = username;
            this.Password = password;
        }

        public string Name { get => name; set => name = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }
}
