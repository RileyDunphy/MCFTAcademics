using MCFTAcademics.DAL;
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

        public static IEnumerable<Grade> GetAllGrades() => GradeDAL.GetAllGrades();

        public static IEnumerable<Grade> GetGradesForInstructor(User staff) =>
            GradeDAL.GetGradesForInstructor(staff);

        /// <summary>
        /// Calculates the final grade.
        /// </summary>
        /// <returns>The grade after a formula is applied to it.</returns>
        /// <remarks>
        /// The current algorithm MCFT uses is if the grade is a supplemental,
        /// the result is maximum 60 regardless if more was earned on the
        /// supplemental.
        /// </remarks>
        // XXX: How do we handle historical versions?
        public decimal CalculateFinalGrade() =>
            Supplemental ? Math.Min(60, GradeAssigned) : GradeAssigned;

        public bool IsPassing() => CalculateFinalGrade() >= 60;
    }
}
