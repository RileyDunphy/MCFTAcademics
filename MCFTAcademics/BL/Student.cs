using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Student
    {
        
        private List<Grade> grades;
        private string firstName;
        private string lastName;
        private string studentId;
        private DateTime admissionDate;

        public Student() { }

        public Student(int id) {
            Student s=StudentDAL.GetStudentByStudentId(id);
            this.grades = s.grades;
            this.firstName = s.firstName;
            this.lastName = s.lastName;
            this.lastName = s.lastName;
            this.admissionDate = s.admissionDate;
            this.studentId = s.studentId;
        }

        //TODO: change this to get grades from db method (once stored procedure is created)
        public Student(List<Grade> grades, string firstName, string lastName, string studentId, DateTime admissionDate)
        {
            this.grades = grades;
            this.firstName = firstName;
            this.lastName = lastName;
            this.studentId = studentId;
            this.admissionDate = admissionDate;
        }

        public DateTime AdmissionDate { get => admissionDate; }
        public string StudentId { get => studentId; }
        public string FirstName { get => firstName; }
        public string LastName { get => lastName; }
        public List<Grade> Grades { get => grades; }

        public static List<Student> GetStudentsByCourseId(int id)
        {
            return StudentDAL.GetStudentsByCourseId(id);
        }

        public Grade GetGradeByCourseId(int id)
        {
            return StudentDAL.GetGradeByCourseId(id, this);
        }

        public static Student GetStudentByStudentId(int id)
        {
            return StudentDAL.GetStudentByStudentId(id);
        }
    }
}
