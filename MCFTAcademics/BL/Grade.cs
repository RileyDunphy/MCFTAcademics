using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Grade
    {
        private int studentId;
        private decimal gradeAssigned;
        private DateTime given;
        private bool locked;
        private decimal hoursAttended;
        private bool supplemental;
        private Course subject;

        public Grade()
        {
        }

        public Grade(int studentId, decimal gradeAssigned, DateTime given, bool locked, decimal hoursAttended, bool supplemental, Course subject)
        {
            this.studentId = studentId;
            this.gradeAssigned = gradeAssigned;
            this.given = given;
            this.locked = locked;
            this.hoursAttended = hoursAttended;
            this.supplemental = supplemental;
            this.subject=subject;
        }

        public int StudentId { get => studentId; }
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
        public static List<Grade> GetAllGrades()
        {
            return GradeDAL.GetAllGrades();
        }
    }
}
