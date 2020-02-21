using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Staff
    {
        public Staff()
        {
        }

        public Staff(int id, string name, string type)
        {
            this.UserId = id;
            this.Name = name;
            this.Type = type;
        }

        public int UserId { get; }
        public string Name { get; }
        public string Type { get; }
        public static List<Staff> GetAllStaff()
        {
            return StaffDAL.GetAllStaff();
        }
    }
}
