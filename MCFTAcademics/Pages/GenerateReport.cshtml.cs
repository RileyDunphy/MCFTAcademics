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

                List<Report> dt = ReportDAL.SelectReportData(program, semester);

               //JsonSerializer js= JsonSerializer.Create();
                //string json = JsonConvert.SerializeObject(dt);
                return new JsonResult(dt);
      
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}