using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Grade : IComparable<Grade> 
    {
        private int studentId;
        private decimal gradeAssigned;
        private DateTime given;
        private bool locked;
        private decimal hoursAttended;
        private bool supplemental;
        private Course subject;
        private string comment;
        private DateTime? unlockedUntil;

        public Grade()
        {
        }

        public Grade(int studentId, decimal gradeAssigned, DateTime given, bool locked, decimal hoursAttended, bool supplemental, Course subject, string comment, DateTime? unlockedUntil)
        {
            this.studentId = studentId;
            this.gradeAssigned = gradeAssigned;
            this.given = given;
            this.locked = locked;
            this.hoursAttended = hoursAttended;
            this.supplemental = supplemental;
            this.subject=subject;
            this.comment = comment;
            this.unlockedUntil = unlockedUntil;
        }

        public int StudentId { get => studentId; }
        public decimal GradeAssigned { get => gradeAssigned;  }
        public DateTime Given { get => given;  }
        public bool Locked { get => locked;  }
        public decimal HoursAttended { get => hoursAttended;  }
        public bool Supplemental { get => supplemental; }
        public Course Subject { get => subject; }
        public string Comment { get => comment; }
        public DateTime? UnlockedUntil { get => unlockedUntil; }

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
        public decimal CalculateFinalGrade()
        {
            // If their grading algorithm changes in the future,
            // please modify the list of grading algorithms and
            // return the old algorithm type for grades in old date ranges,
            // to avoid issues with recalculating in the future
            switch (GetGradingAlgorithm()) {
                case GradingAlgorithm.Default:
                default:
                    return Supplemental ? Math.Min(60, GradeAssigned) : GradeAssigned;
            }
        }
        public bool IsPassing() => CalculateFinalGrade() >= 60;

        internal GradingAlgorithm GetGradingAlgorithm()
        {
            // Check based on time (or other criteria?)
            return GradingAlgorithm.Default;
        }

        public static bool ToggleGradeLock(int studentId, int courseId, DateTime? unlockedUntil)
        {
            return GradeDAL.ToggleGradeLock(studentId, courseId, unlockedUntil);
        }

        public static bool UpdateGrade(Grade grade, int studentId)
        {
            return GradeDAL.UpdateGrade(grade, studentId);
        }

        int IComparable<Grade>.CompareTo(Grade other)
        {
            Course thisCourse=this.subject;
            Course otherCourse = other.subject;

            return thisCourse.CompareTo(otherCourse);
        }
 
        public static Grade GetSummerPracticum(Student s)
        {
            return GradeDAL.GetSummerPracticum(s);
        }
    }
}
