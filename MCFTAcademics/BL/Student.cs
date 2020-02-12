using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Student
    {
        public Student(int id, string firstName, string lastName, string studentCode, string program, DateTime admissionDate)
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
        public DateTime AdmissionDate { get; }
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

        public static List<Student> GetStudentsByCourseId(Course course)
        {
            return StudentDAL.GetStudentsInCourse(course);
        }
    }
}
