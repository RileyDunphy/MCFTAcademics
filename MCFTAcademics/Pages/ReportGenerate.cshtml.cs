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
            generateReport();



            return RedirectToPage("Index");
        }

            public static void generateReport() 
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

            Console.Write(gfx.ToString());


            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.Center);
            gfx.DrawString("Any String", font, XBrushes.DarkBlue, new XRect(0, 50, page.Width, page.Height), XStringFormats.Center);

            // Save the document...
            
            const string filename = "HelloWorld.pdf";
            //this just saves the document into the project folder
            document.Save(filename);
            // ...and start a viewer.
            //Process.Start(filename);
            //}catch(Exception ex){

            //}
        }


    }




            
        
}
