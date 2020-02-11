using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;
using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using MCFTAcademics.BL;
using System.IO;
using Rotativa;
using System.Data;

namespace MCFTAcademics
{
    public class GradeByStudentId : PageModel
    {
        public void SubmitBtn_Click()
        {
            int id = Convert.ToInt32(Request.Form["studentId"]);

        }
        public IActionResult OnGet() {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                string somestrang="SomeString";
                
            }
            catch (Exception ex) { }
            return null;

        }
        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                int id = Convert.ToInt32(Request.Form["studentId"]);

                List<Grade> grades=StudentDAL.GetGradeByStudentId(id);
                foreach (Grade g in grades) {
                    Console.WriteLine(g.ToString());

                }



            }
            catch (Exception ex) { }
            return null;
        }
    }


            
        
}
