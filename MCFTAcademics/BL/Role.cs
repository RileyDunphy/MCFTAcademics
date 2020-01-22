using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Role
    {
        private string name;

        public Role()
        {
        }

        public Role(string name)
        {
            this.Name = name;
        }

        public string Name { get => name; set => name = value; }
    }
}
