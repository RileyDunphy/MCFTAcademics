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


namespace MCFTAcademics
{
    public class ReportGenerate : PageModel
    {


        public async Task<IActionResult> OnPostAsync()
        {
            //can now name the report by passing in different arguments
            generateReport("reportName");


            return RedirectToPage("index");


        }

            public static void generateReport(string reportName) 
        {
            //try { 
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("arial", 20, XFontStyle.Bold);

            




            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.Center);
            gfx.DrawString("Any String", font, XBrushes.DarkBlue, new XRect(0, 50, page.Width, page.Height), XStringFormats.Center);

            // Save the document... must be a const
            const string filename = "./Reports/someReport.pdf";

            if (System.IO.Directory.Exists("./Reports")) {

                document.Save(filename);

                if (System.IO.File.Exists("./Reports/someReport.pdf")) { 

                    System.IO.File.Move("./Reports/someReport.pdf", "./Reports/"+reportName+".pdf");

                }
            }
            // ...and start a viewer.
            //Process.Start(filename);
            //}catch(Exception ex){

            //}
        }


    }




            
        
}
