using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;

namespace MCFTAcademics.BL.Reports
{
    public class Report
    {
        //private List<Grade> grades= new List<Grade>();
        private List<ReportColumn> columns;

        public Report(List<ReportColumn> columns)
        {
            this.Columns = columns;
        }
        public Report(string program, int semester) {
            this.columns = ReportDAL.SelectReportData(program, semester);
            fillGrades();
        }

        //public List<Grade> Grades { get => grades; set => grades = value; }
        public List<ReportColumn> Columns { get => columns; set => columns = value; }

        public bool fillGrades()
        {
            try
            {
                //sort by student ID
                columns.Sort();

                //some fields are hard coded as they are not used in reports
                //foreach (ReportColumn col in columns)
                //{
                //    int studentId = col.StudentId;
                //    decimal gradeAssigned = col.Grade;
                //    DateTime given = DateTime.Now;
                //    bool locked = false;
                //    decimal hoursAttended = -1;
                //    bool supplemental = col.Supplemental;
                //    Course subject = Course.GetCourseById(col.CourseId);

                //    //Grade Grade = new Grade(studentId, gradeAssigned, given, locked, hoursAttended, supplemental, subject);

                //    //grades.Add(Grade);
                //}

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}