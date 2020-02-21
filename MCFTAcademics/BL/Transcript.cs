using Microsoft.AspNetCore.Http;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCFTAcademics.DAL;

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
        private string type;

        public Transcript(Student student, bool official, string signatureName, string type, DateTime creationDate)
        {
            this.student = student;
            this.official = official;
            this.signatureName = signatureName;
            this.creationDate = DateTime.Now;
            this.type = type;
            reportName = student.StudentCode
                + "_"
                + creationDate.ToShortDateString()
                + "_"
                + creationDate.TimeOfDay.ToString().Replace('.','_').Replace(':','_');
            //I had to replace the . in the time of day string so it didn't interfere with the filename saving
        }

        public string GenerateTranscript()
        {
            //try { 
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";
            string path=null;

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            //XFont font = new XFont("arial", 20, XFontStyle.Bold);
            XFont font=new XFont("arial", 11, XFontStyle.Bold);
            XFont smallFont = new XFont("arial", 10);

            // Draw the header at the top
            gfx.DrawString("TRANSCRIPT OF ACADEMIC RECORD",font,XBrushes.Black,new XRect(275,0,300,100),XStringFormats.Center);
            gfx.DrawRectangle(XPens.Black, new XRect(275, 65, 300, 100));
            gfx.DrawString("Student: "+this.student.FirstName+" "+this.student.LastName,smallFont,XBrushes.Black,new XRect(280,75,page.Width,page.Height),XStringFormats.TopLeft);
            gfx.DrawString("ID: " + this.student.StudentCode, smallFont, XBrushes.Black, new XRect(280, 90, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Class Year :" + this.student.GraduationDate.Value.Year, smallFont, XBrushes.Black, new XRect(280, 105, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Date Graduated: " + this.student.GraduationDate.Value.ToString("MMMM dd, yyyy"), smallFont, XBrushes.Black, new XRect(280, 120, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Class Rank: " + this.student.GetClassRank(), smallFont, XBrushes.Black, new XRect(280, 135, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Program: " + this.student.Program, smallFont, XBrushes.Black, new XRect(280, 150, page.Width, page.Height), XStringFormats.TopLeft);
            //Address for the header
            gfx.DrawString("Office of the Registrar, 1350 Regent St, Fredericton, NB", smallFont, XBrushes.Black, new XRect(10, 135, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("E3C 2G6, (506) 458 0653 or 866 619 9900 www.mcft.ca", smallFont, XBrushes.Black, new XRect(10, 150, page.Width, page.Height), XStringFormats.TopLeft);


            //Main section of the page
            gfx.DrawRectangle(XPens.Black, new XRect(10, 175, 565, 650));
            if (this.type == "Program" || this.type == "Year 1" || this.type == "Semester 1")
            {
                gfx.DrawString("YEAR 1 - SEMESTER I", font, XBrushes.Black, new XRect(15, 190,page.Width,page.Height), XStringFormats.TopLeft);
                gfx.DrawString("SEPTEMBER TO DECEMBER " + (Convert.ToInt32(this.student.GraduationDate.Value.Year)-2), font, XBrushes.Black, new XRect(15, 200, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("COURSE", font, XBrushes.Black, new XRect(15, 215, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("CLASS GRADE", font, XBrushes.Black, new XRect(270, 215, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("FIELD GRADE", font, XBrushes.Black, new XRect(360, 215, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("COURSE GRADE", font, XBrushes.Black, new XRect(450, 215, 50, 50), XStringFormats.TopLeft);
                int y = 230;
                foreach(Grade g in this.student.GetGrades())//All the students grades for semester 1
                {
                    gfx.DrawString(g.Subject.Name, smallFont, XBrushes.Black, new XRect(15, y, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(g.GradeAssigned.ToString(), smallFont, XBrushes.Black, new XRect(450, y, 50, 15), XStringFormats.Center);
                    y+= 15;
                }
                y += 10;
                gfx.DrawString("*Indicates grade achieved through supplemental exam", font, XBrushes.Black, new XRect(15, y, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Semester Average: ", font, XBrushes.Black, new XRect(330, y, page.Width, page.Height), XStringFormats.TopLeft);
            }

            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.Center);
            string gradeContent="";
            int height = 50;

            foreach (Grade g in this.student.GetGrades()) {
                gradeContent = "is Supplemental: "+g.Supplemental.ToString() +g.Subject.Name + "--Grade " + g.GradeAssigned.ToString();
                gfx.DrawString(gradeContent, smallFont, XBrushes.DarkBlue, new XRect(0, height, page.Width, page.Height), XStringFormats.Center);
                height += 25;
            }

            
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
                        path = "./Reports/" + this.reportName + ".pdf";
                        string oldPath= "./Reports/Transcript.pdf";
                        System.IO.File.Move(oldPath, path);
                        TranscriptDAL.AddTranscript(path, student);

                    }
                }
                catch (Exception ex) {
                    Console.Write(ex.Message);
                }
                return path;

            }
            return path;

        }
    }
}
