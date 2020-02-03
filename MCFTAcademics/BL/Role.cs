using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Role
    {
        // yeah, this class is weird because it's a m2n relationship packed into one of its relations
        // but that's a DB side thing
        public Role(string name, int id, User user)
        {
            this.Name = name;
            this.Id = id;
            this.User = user;
        }

        // the user should no longer use this object afterward
        public bool Revoke() => RoleDAL.Revoke(this);

        public string Name { get; }
        public int Id { get; }
        public User User { get; }
    }
}
