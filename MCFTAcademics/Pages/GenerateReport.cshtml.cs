using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MCFTAcademics.BL;

namespace MCFTAcademics
{
    public class GenerateReportModel : PageModel
    {
        public void OnGet()
        {

        }
        public ActionResult OnGetAjax(string program, int semester, int year)
        {
            try
            {

                Report report = new Report(program, semester, year);

                string path = report.generateReport();

                
                path = path.Replace("./Reports/", "");
                path = path.Replace(".pdf", "");
                return new JsonResult(path);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

    }
}