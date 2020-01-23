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
            this.name = name;
            this.username = username;
            this.password = password;
        }

        public string Name { get => name;}
        public string Username { get => username; }
        public string Password { get => password;  }
    }
}
