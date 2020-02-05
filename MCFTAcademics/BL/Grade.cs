using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Grade
    {

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

        public decimal GradeAssigned { get;  }
        public DateTime Given { get;  }
        public bool Locked { get;  }
        public decimal HoursAttended { get;  }
        public bool Supplemental { get; }

        public bool IsPassing()
        {
            if (this.GradeAssigned >= 60)
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
