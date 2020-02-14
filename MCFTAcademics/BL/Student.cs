using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Student
    {
        public Student(int id, string firstName, string lastName, string studentCode, string program, DateTime? admissionDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            StudentCode = studentCode;
            Program = program;
            AdmissionDate = admissionDate;
        }

        /// <summary>
        /// This is the student ID used as the database primary key.
        /// </summary>
        public int Id { get; }
        // XXX: This can be nullable in the DB, is that right?
        public DateTime? AdmissionDate { get; }
        /// <summary>
        /// This is the student ID that MCFT uses for display.
        /// (not the database PK)
        /// </summary>
        public string StudentCode { get; }
        public string FirstName { get; }
        public string LastName { get; }
        // XXX: Since this and Course have this, does this need to be its own
        // type or table?
        public string Program { get; }

        // XXX: Does this make sense to put in here, or does it go in Grade?
        public IEnumerable<Grade> GetGrades()
        {
            return GradeDAL.GetGradesForStudent(this);
        }

        public Grade GetGradeForCourse(Course course)
        {
            return GradeDAL.GetGradesForStudentInCourse(course, this);
        }

        public static Student GetStudent(int id)
        {
            return StudentDAL.GetStudent(id);
        }

        public static List<Student> GetStudentsByCourse(Course course)
        {
            return StudentDAL.GetStudentsInCourse(course);
        }

        /// <summary>
        /// Creates a student code for display purposes.
        /// </summary>
        /// <param name="year">The two-digit year.</param>
        /// <param name="id">A three-digit student ID.</param>
        /// <returns>The student code used for display.</returns>
        /// <remarks>
        /// The ID has no relation to the database key.
        /// </remarks>
        public static string GenerateStudentCode(int year, int id = 0)
        {
            if (year < 0 || year > 99)
                throw new ArgumentException("Year must be two-digit positive number", nameof(year));
            if (id > 999 || id < 0)
                throw new ArgumentException("ID must be three-digit positive number", nameof(id));

            // pad with zeros
            var studentCode = $"S{year:D2}{id:D3}";
            // XXX: try something less intensive, like using 
            if (StudentDAL.GetStudent(studentCode) != null)
            {
                // it exists, try again
                return GenerateStudentCode(year, id++);
            }
            return studentCode;
        }

        public static Student GetStudentByStudentId(int id)
        {
            return StudentDAL.GetStudent(id);
        }
    }
}
