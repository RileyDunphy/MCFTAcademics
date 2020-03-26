using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Student
    {
        //default constructor is needed for serialization
        public Student() { }
        //Needed a constructor without an ID for inserting a student since the ID auto generates from database
        //And admission date wasn't needed either because just grabbed the current time
        public Student(string firstName, string lastName, string studentCode, string program, DateTime? graduationDate, bool academicAccommodation)
        {
            FirstName = firstName;
            LastName = lastName;
            StudentCode = studentCode;
            Program = program;
            GraduationDate = graduationDate;
            AcademicAccommodation = academicAccommodation;
        }
        public Student(int id, string firstName, string lastName, string studentCode, string program, DateTime? admissionDate, DateTime? graduationDate, bool academicAccommodation)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            StudentCode = studentCode;
            Program = program;
            AdmissionDate = admissionDate;
            GraduationDate = graduationDate;
            AcademicAccommodation = academicAccommodation;
        }

        /// <summary>
        /// This is the student ID used as the database primary key.
        /// </summary>
        public int Id { get;  }
        public bool AcademicAccommodation { get; set; }
        // XXX: This can be nullable in the DB, is that right?
        public DateTime? AdmissionDate { get; }
        public DateTime? GraduationDate { get; set; }
        /// <summary>
        /// This is the student ID that MCFT uses for display.
        /// (not the database PK)
        /// </summary>
        public string StudentCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // XXX: Since this and Course have this, does this need to be its own
        // type or table?
        public string Program { get; set; }

        // XXX: Does this make sense to put in here, or does it go in Grade?
        public IEnumerable<Grade> GetGrades()
        {
            return GradeDAL.GetGradesForStudent(this);
        }

        public Grade GetGradeForCourse(Course course)
        {
            return GradeDAL.GetGradesForStudentInCourse(course, this);
        }

        public static Student GetStudent(string code)
        {
            return StudentDAL.GetStudent(code);
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

        public Grade GetGradeFromListByCourseId(int id,IEnumerable<Grade> grades)
        {
            foreach (Grade g in grades) {
                if (g.Subject.Id == id) {
                    return g;
                }
            }
            return null;
            
        }

        public static List<Student> GetAllStudents()
        {
            return StudentDAL.GetAllStudents();
        }

        public string GetClassRank()
        {
            return StudentDAL.GetClassRank(this);
        }

        public IEnumerable<Grade> GetGradesForSemester(int semester)
        {
            return GradeDAL.GetGradesForStudentSemester(this,semester);
        }

        public decimal GetAverageForSemester(int semester)//Probably use the formula building way instead of this now
        {
            return GradeDAL.GetAverageForStudentSemester(this, semester);
        }
        public static decimal GetAverageForSemesterByStudentId(int studentId, int semester)
        {
            return GradeDAL.GetAverageForStudentSemester(Student.GetStudent(studentId), semester);
        }
        public decimal GetAverage(int semester = -1)
        {//Calls the method that uses the formula
            return GradeDAL.GetAverageForStudent(this,semester);
        }

        public int AddStudent()
        {
            return StudentDAL.AddStudent(this);
        }
        public static decimal GetAverageByStudentId(int studentId, int semester = -1)
        {//Calls the method that uses the formula
            return GradeDAL.GetAverageForStudent(Student.GetStudent(studentId), semester);
        }
        //TODO
        //XXX
        public static decimal GetAverageByStudentIdYearly(int studentId, int year, int semester = -1)
        {
            return GradeDAL.GetAverageForStudent(Student.GetStudent(studentId), semester);
        }

    }
}
