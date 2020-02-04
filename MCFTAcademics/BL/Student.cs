using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{
    public class Student
    {
        
        private List<Grade> grades;
        private string name;
        private string studentId;
        private DateTime admissionDate;

        public Student() { }
        //TODO: change this to get grades from db method (once stored procedure is created)
        public Student(List<Grade> grades, string name, string studentId, DateTime admissionDate)
        {
            this.grades = grades;
            this.name = name;
            this.studentId = studentId;
            this.admissionDate = admissionDate;
        }

        public DateTime AdmissionDate { get => admissionDate; }
        public string StudentId { get => studentId; }
        public string Name { get => name; }
        public List<Grade> Grades { get => grades; }


    }
}
