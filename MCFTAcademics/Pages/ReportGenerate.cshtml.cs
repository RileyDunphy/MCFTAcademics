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

namespace MCFTAcademics
{
    public class ReportGenerate : PageModel
    {
        
        public async Task<IActionResult> OnPostAsync()
        {
            //can now name the report by passing in different arguments
            //generateReport("reportName");
            Grade math = new Grade(95, DateTime.Now, false, 20, false, new Course(1, "Math", 1.2m, "MathTest",3,3,3,9,12,"maths",true,null));
            Grade chainsaw = new Grade(95, DateTime.Now, false, 20, false, new Course(2,"Chainsaw", 1.2m, "this is for trees",1,2,3,4,1.5m,"forestry",true,null));
            List<Grade> grades = new List<Grade>();

            grades.Add(math);
            grades.Add(chainsaw);

            Student josh = new Student(grades,"Josh", "1234", DateTime.Now);

            Transcript t = new Transcript(josh,false, "Riley Dunphy", DateTime.Now);

            PdfDocument document= Transcript.generateReport(t);

            // Send PDF to browser
            //MemoryStream stream = new MemoryStream();
            //document.Save(stream, false);
            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-length", stream.Length.ToString());
            //Response.BinaryWrite(stream.ToArray());
            //Response.Flush();
            //stream.Close();
            //Response.End();
            return null;
            
        }


    }




            
        
}
