using Microsoft.AspNetCore.Http;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.IO;
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
            reportName = student.StudentCode
                + "_"
                + creationDate.ToShortDateString()
                + "_"
                + creationDate.TimeOfDay;
        }

        public static PdfDocument generateReport(Transcript t)
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
            XFont smallFont=new XFont("arial", 7, XFontStyle.Bold);

            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.Center);
            string gradeContent="";
            int height = 50;

            foreach (Grade g in t.student.GetGrades()) {
                gradeContent = "is Supplemental: "+g.Supplemental.ToString() +g.Subject.Name + "--Grade " + g.GradeAssigned.ToString();
                gfx.DrawString(gradeContent, smallFont, XBrushes.DarkBlue, new XRect(0, height, page.Width, page.Height), XStringFormats.Center);
                height += 25;
            }

            //gfx.DrawString(gradeContent, font, XBrushes.DarkBlue, new XRect(0, 50, page.Width, page.Height), XStringFormats.Center);
            gfx.DrawString(t.student.FirstName + " " + t.student.LastName, font, XBrushes.DarkBlue, new XRect(0, 50, page.Width, page.Height), XStringFormats.TopCenter);
            
            // Save the document... must be a const
            const string filename = "./Reports/Transcript.pdf";

            if (System.IO.Directory.Exists("./Reports"))
            {

                // Send PDF to browser
                MemoryStream stream = new MemoryStream();
                document.Save(stream, false);



                document.Save(filename);
                
                //this is to rname the document
                try
                {
                    if (System.IO.File.Exists("./Reports/Transcript.pdf"))
                    {
                        string path = "/Reports/" + t.reportName + ".pdf";
                        string oldPath= "/Reports/Transcript.pdf";
                        System.IO.File.Move(oldPath, path);

                    }
                }
                catch (Exception ex) {
                    Console.Write(ex.Message);
                }
                return document;

            }
            return null;

        }
    }
}
