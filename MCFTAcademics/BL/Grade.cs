using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Grade
    {
        private decimal gradeAssigned;
        private DateTime given;
        private bool locked;
        private decimal hoursAttended;
        private bool supplemental;

        public Grade()
        {
        }

        public Grade(decimal gradeAssigned, DateTime given, bool locked, decimal hoursAttended, bool supplemental)
        {
            this.GradeAssigned = gradeAssigned;
            this.Given = given;
            this.Locked = locked;
            this.HoursAttended = hoursAttended;
            this.Supplemental = supplemental;
        }

        public decimal GradeAssigned { get => gradeAssigned; set => gradeAssigned = value; }
        public DateTime Given { get => given; set => given = value; }
        public bool Locked { get => locked; set => locked = value; }
        public decimal HoursAttended { get => hoursAttended; set => hoursAttended = value; }
        public bool Supplemental { get => supplemental; set => supplemental = value; }

        public bool isPassing()
        {
            if (this.gradeAssigned >= 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
