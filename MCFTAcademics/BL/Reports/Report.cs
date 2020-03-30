using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;

namespace MCFTAcademics.BL.Reports
{
    public class Report
    {
        private bool semesterReport = true;
        private List<ReportColumn> columns=new List<ReportColumn>();
        private int semester;
        private int year;

        public List<ReportColumn> Columns { get => columns; set => columns = value; }
        public bool SemesterReport { get => semesterReport; set => semesterReport = value; }
        public int Semester { get => semester; set => semester = value; }
        public int Year { get => year; set => year = value; }

        public Report(List<ReportColumn> columns)
        {
            this.Columns = columns;
        }
        
        public Report(string program, int semester,int year) {
            this.Semester = semester;
            this.Year = year;

            if (semester == 0 && year == 0)
            {
                SemesterReport = false;

                List<ReportColumn> semester1 = ReportDAL.SelectReportData(program, 1, SemesterReport);
                List<ReportColumn> semester2 = ReportDAL.SelectReportData(program, 2, SemesterReport);
                List<ReportColumn> semester3 = ReportDAL.SelectReportData(program, 3, SemesterReport);
                List<ReportColumn> semester4 = ReportDAL.SelectReportData(program, 4, SemesterReport);
                List<ReportColumn> semester5 = ReportDAL.SelectReportData(program, 5, SemesterReport);
                List<ReportColumn> semester6 = ReportDAL.SelectReportData(program, 6, SemesterReport);

                List<List<ReportColumn>> semesters = new List<List<ReportColumn>>();

                semesters.Add(semester1);
                semesters.Add(semester2);
                semesters.Add(semester3);
                semesters.Add(semester4);
                semesters.Add(semester5);
                semesters.Add(semester6);

                try
                {
                    foreach (List<ReportColumn> s in semesters)
                    {
                        foreach (ReportColumn col in s)
                        {
                            this.columns.Add(col);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ex.StackTrace.ToCharArray();
                }
            }
            else if (semester == 0 && year != 0) {
                SemesterReport = false;

                List<ReportColumn> semester1 = ReportDAL.SelectReportDataYearly(program, 1, year, SemesterReport);
                List<ReportColumn> semester2 = ReportDAL.SelectReportDataYearly(program, 2, year, SemesterReport);
                List<ReportColumn> semester3 = ReportDAL.SelectReportDataYearly(program, 3, year, SemesterReport);
                List<ReportColumn> semester4 = ReportDAL.SelectReportDataYearly(program, 4, year, SemesterReport);
                List<ReportColumn> semester5 = ReportDAL.SelectReportDataYearly(program, 5, year, SemesterReport);
                List<ReportColumn> semester6 = ReportDAL.SelectReportDataYearly(program, 6, year, SemesterReport);

                List<List<ReportColumn>> semesters = new List<List<ReportColumn>>();

                semesters.Add(semester1);
                semesters.Add(semester2);
                semesters.Add(semester3);
                semesters.Add(semester4);
                semesters.Add(semester5);
                semesters.Add(semester6);

                try
                {
                    foreach (List<ReportColumn> s in semesters)
                    {
                        foreach (ReportColumn col in s)
                        {
                            this.columns.Add(col);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ex.StackTrace.ToCharArray();
                }

            }
            else
            {
                this.columns = ReportDAL.SelectReportData(program, semester, semesterReport);
            }
            
            //sort by student ID (keeping student details together)
                columns.Sort();
                getAverages();
        }

        private void getAverages() {
            if (!semesterReport)
            {
                decimal average = 0;
                int studentId = 0;
                foreach (ReportColumn col in this.columns)
                {
                    if (col.StudentId == studentId)
                    {
                        col.Average = average;
                    }
                    else
                    {
                        studentId = col.StudentId;
                        average = Student.GetAverageByStudentId(studentId);
                        col.Average = average;
                    }
                }
            }
            else {
                decimal average = 0;
                int studentId = 0;
                foreach (ReportColumn col in this.columns)
                {
                    if (col.StudentId == studentId)
                    {
                        col.Average = average;
                    }
                    else
                    {
                        studentId = col.StudentId;
                        average = Student.GetAverageForSemesterByStudentId(studentId,semester);
                        col.Average = average;
                    }
                }

            }
        
        }

    }
}