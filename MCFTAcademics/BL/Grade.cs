﻿using System;
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
        private Course subject;

        public Grade()
        {
        }

        public Grade(decimal gradeAssigned, DateTime given, bool locked, decimal hoursAttended, bool supplemental, Course subject)
        {
            this.gradeAssigned = gradeAssigned;
            this.given = given;
            this.locked = locked;
            this.hoursAttended = hoursAttended;
            this.supplemental = supplemental;
            this.subject=subject;
        }

        public decimal GradeAssigned { get => gradeAssigned;  }
        public DateTime Given { get => given;  }
        public bool Locked { get => locked;  }
        public decimal HoursAttended { get => hoursAttended;  }
        public bool Supplemental { get => supplemental; }
        public Course Subject { get => subject; }

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
