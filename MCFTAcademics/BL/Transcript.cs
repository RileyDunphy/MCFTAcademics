using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{/// <summary>
/// This class will be used to generate Transcripts for students
/// </summary>
    public class Transcript
    {
        private Student student;
        private bool official;
        private string signatureName;
        private DateTime creationDate;
        private string reportName;

        public Transcript(Student student, bool official, string signatureName, DateTime creationDate)
        {
            this.student = student;
            this.official = official;
            this.signatureName = signatureName;
            this.creationDate = DateTime.Now;
            this.reportName = student.StudentId+"_"+ creationDate.TimeOfDay;
        }

        public void generateReport()
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

            if (System.IO.Directory.Exists("./Reports"))
            {

                document.Save(filename);

                if (System.IO.File.Exists("./Reports/someReport.pdf"))
                {

                    System.IO.File.Move("./Reports/someReport.pdf", "./Reports/" + this.reportName + ".pdf");

                }
            }
            
        }

    }
}
