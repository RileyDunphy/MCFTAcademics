using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MCFTAcademics.BL;
using System.Data;
using Newtonsoft.Json;
using MCFTAcademics.DAL;
using MCFTAcademics.BL.Reports;

namespace MCFTAcademics
{
    public class GenerateReportModel : PageModel
    {
        public void OnGet()
        {

        }
        public IActionResult OnGetAjax(string program, int semester, int year)
        {
            try
            {
                Report rpt = new Report(program,semester);
                List<Grade>grades=rpt.Grades;
                List<string> courseCodes = new List<string>();
                List<string> ids = new List<string>();



                foreach (ReportColumn col in rpt.Columns) {
                    string courseCode = col.CourseCode;
                    if (!courseCodes.Contains(courseCode)) {
                        courseCodes.Add(courseCode);
                    }
                    int studentId = col.StudentId;
                    if (!ids.Contains(studentId.ToString()))
                    {
                        ids.Add(studentId.ToString());
                    }
                }
                int length=courseCodes.Count;
                int width = ids.Count;

                int[,] gradesArray = new int[length, width];
                string[,] gradeMap = new string[length, width];
                int i = 0;
                foreach(string cc in courseCodes)
                {
                //    gradeMap[0, i] = cc;
                    i++;
                }
                i = 0;
                foreach (string id in ids)
                {
                //    gradeMap[i, 0] = id;
                    i++;
                }

                foreach (ReportColumn col in rpt.Columns) {
                    
                
                }

                

                return new JsonResult(rpt);
      
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}