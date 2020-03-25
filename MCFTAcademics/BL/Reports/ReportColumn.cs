using System;
using System.Collections;
using System.Collections.Generic;

namespace MCFTAcademics.BL
{/// <summary>
/// This class will be used to hold data for generating academic reports for the college.
/// </summary>
    public class ReportColumn : IComparable<ReportColumn>
    {
        private int studentId;
        private string firstName;
        private string lastName;
        private decimal grade;
        private bool supplemental;
        private int courseId;
        private string name;
        private string program;
        private string courseCode;
        private DateTime startDate;
        private DateTime endDate;
        private decimal average;
        
        public ReportColumn(int studentId, string firstName, string lastName, decimal grade, bool supplemental, int courseId, string name, string program, string courseCode, DateTime startDate, DateTime endDate,int semester, bool semesterReport)
        {
            this.studentId = studentId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.grade = grade;
            this.supplemental = supplemental;
            this.courseId = courseId;
            this.name = name;
            this.program = program;
            this.courseCode = courseCode;
            this.startDate = startDate;
            this.endDate = endDate;
            if (semesterReport)
            {
                this.average = Student.GetAverageForSemesterByStudentId(studentId, semester);
            }
            else 
            {
                this.average = Student.GetAverageByStudentId(studentId);
            }
            
        }

        public int StudentId { get => studentId;  }
        public string FirstName { get => firstName;  }
        public string LastName { get => lastName;  }
        public decimal Grade { get => grade;  }
        public bool Supplemental { get => supplemental;  }
        public int CourseId { get => courseId; }
        public string Name { get => name;  }
        public string Program { get => program;  }
        public string CourseCode { get => courseCode;  }
        public DateTime StartDate { get => startDate; }
        public DateTime EndDate { get => endDate; }
        public decimal Average { get => average; }

        public int CompareTo(ReportColumn obj)
        {
            return studentId.CompareTo(obj.StudentId);
        }
    }
}