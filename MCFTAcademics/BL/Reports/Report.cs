using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;

namespace MCFTAcademics.BL.Reports
{
    public class Report
    {
        
        private List<ReportColumn> columns;
        public List<ReportColumn> Columns { get => columns; set => columns = value; }
        public Report(List<ReportColumn> columns)
        {
            this.Columns = columns;
        }
        public Report(string program, int semester) {
            this.columns = ReportDAL.SelectReportData(program, semester);
            //sort by student ID (keeping student details together)
                columns.Sort();
        }

    }
}