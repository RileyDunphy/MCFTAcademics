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
        public List<int> years=new List<int>();
        public void OnGet()
        {
            years=Grade.GetDateRanges();
            
        }
        public IActionResult OnGetAjax(string program, int semester, int year)
        {
            try
            {
                Report rpt = new Report(program, semester);
               
                List<string> courseCodes = new List<string>();
                List<string> ids = new List<string>();

                return new JsonResult(rpt);
      
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}