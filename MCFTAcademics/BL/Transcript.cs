using MCFTAcademics.DAL;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.IO;

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
            this.reportName = student.StudentCode
                + creationDate.ToShortDateString()
                + creationDate.TimeOfDay.ToString().Replace('.', '_').Replace(':', '_');
            //I had to replace the . in the time of day string so it didn't interfere with the filename saving
        }

        public string GenerateTranscript()
        {
            
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Transcript";
            string path = null;

            // Create an empty page
            PdfPage page1 = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page1);

            // Create a font
            XFont font = new XFont("arial", 11, XFontStyle.Bold);
            XFont smallFont = new XFont("arial", 10);

            // Draw the header at the top
            gfx.DrawString("TRANSCRIPT OF ACADEMIC RECORD", font, XBrushes.Black, new XRect(275, 0, 300, 100), XStringFormats.Center);
            gfx.DrawRectangle(XPens.Black, new XRect(275, 65, 300, 100));
            gfx.DrawString("Student: " + this.student.FirstName + " " + this.student.LastName, smallFont, XBrushes.Black, new XRect(280, 75, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("ID: " + this.student.StudentCode, smallFont, XBrushes.Black, new XRect(280, 90, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("Class Year: " + this.student.GraduationDate.Value.Year, smallFont, XBrushes.Black, new XRect(280, 105, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("Date Graduated: " + this.student.GraduationDate.Value.ToString("MMMM dd, yyyy"), smallFont, XBrushes.Black, new XRect(280, 120, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("Class Rank: " + this.student.GetClassRank(), smallFont, XBrushes.Black, new XRect(280, 135, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("Program: " + this.student.Program, smallFont, XBrushes.Black, new XRect(280, 150, page1.Width, page1.Height), XStringFormats.TopLeft);
            //Address for the header
            gfx.DrawString("Office of the Registrar, 1350 Regent St, Fredericton, NB", smallFont, XBrushes.Black, new XRect(10, 135, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("E3C 2G6, (506) 458 0653 or 866 619 9900 www.mcft.ca", smallFont, XBrushes.Black, new XRect(10, 150, page1.Width, page1.Height), XStringFormats.TopLeft);

            //Main section of the page
            gfx.DrawRectangle(XPens.Black, new XRect(10, 175, 565, 650));
            //SEMESTER 1 Portion of page if "Year 1" Transcript or "Program" Transcript
            if (this.type == "Program" || this.type == "Year 1" || this.type == "Semester 1")
            {
                gfx.DrawString("YEAR 1 - SEMESTER I", font, XBrushes.Black, new XRect(15, 190, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("SEPTEMBER TO DECEMBER " + (Convert.ToInt32(this.student.GraduationDate.Value.Year) - 2), font, XBrushes.Black, new XRect(15, 200, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("COURSE", font, XBrushes.Black, new XRect(15, 215, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("CLASS GRADE", font, XBrushes.Black, new XRect(270, 215, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("FIELD GRADE", font, XBrushes.Black, new XRect(360, 215, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("COURSE GRADE", font, XBrushes.Black, new XRect(450, 215, 50, 50), XStringFormats.TopLeft);
                int y = 230;
                foreach (Grade g in this.student.GetGradesForSemester(1))//All the students grades for semester 1
                {
                    gfx.DrawString(g.Subject.Name, smallFont, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                    gfx.DrawString(g.GradeAssigned.ToString(), smallFont, XBrushes.Black, new XRect(450, y, 50, 15), XStringFormats.Center);
                    y += 15;
                }
                y += 10;
                gfx.DrawString("*Indicates grade achieved through supplemental exam", font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("Semester Average: " + this.student.GetAverageForSemester(1), font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                //Just a theory on how this might work
                string honors = this.student.GetAverageForSemester(1) >= 80 ? "Yes" : "No";
                y += 10;
                gfx.DrawString("Academic Honors: " + honors , font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
            }

            if (this.type == "Program" || this.type == "Year 1")
            {
                gfx.DrawString("YEAR 1 - SEMESTER II", font, XBrushes.Black, new XRect(15, 420, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("JANUARY TO APRIL " + (Convert.ToInt32(this.student.GraduationDate.Value.Year) - 1), font, XBrushes.Black, new XRect(15, 430, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("COURSE", font, XBrushes.Black, new XRect(15, 445, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("CLASS GRADE", font, XBrushes.Black, new XRect(270, 445, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("FIELD GRADE", font, XBrushes.Black, new XRect(360, 445, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("COURSE GRADE", font, XBrushes.Black, new XRect(450, 445, 50, 50), XStringFormats.TopLeft);
                int y = 460;
                foreach (Grade g in this.student.GetGradesForSemester(2))//All the students grades for semester 2
                {
                    gfx.DrawString(g.Subject.Name, smallFont, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                    gfx.DrawString(g.GradeAssigned.ToString(), smallFont, XBrushes.Black, new XRect(450, y, 50, 15), XStringFormats.Center);
                    y += 15;
                }
                y += 10;
                gfx.DrawString("*Indicates grade achieved through supplemental exam", font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("Semester Average: " + this.student.GetAverageForSemester(2), font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                string honors = this.student.GetAverageForSemester(2) >= 80 ? "Yes" : "No";
                y += 10;
                gfx.DrawString("Academic Honors: " + honors, font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
            }
            if(this.type == "Program")
            {
                gfx.DrawString("SUMMER PRACTICUM: ", font, XBrushes.Black, new XRect(15, 750, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("SUMMER JOB RATING: ", font, XBrushes.Black, new XRect(15, 765, page1.Width, page1.Height), XStringFormats.TopLeft);
            }
            if (this.type == "Program")
            {
                // Create an empty second page
                PdfPage page2 = document.AddPage();

                // Get an XGraphics object for drawing
                gfx = XGraphics.FromPdfPage(page2);

                // Draw the header at the top
                gfx.DrawString("TRANSCRIPT OF ACADEMIC RECORD", font, XBrushes.Black, new XRect(275, 0, 300, 100), XStringFormats.Center);
                gfx.DrawRectangle(XPens.Black, new XRect(275, 65, 300, 100));
                gfx.DrawString("Student: " + this.student.FirstName + " " + this.student.LastName, smallFont, XBrushes.Black, new XRect(280, 75, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("ID: " + this.student.StudentCode, smallFont, XBrushes.Black, new XRect(280, 90, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("Class Year: " + this.student.GraduationDate.Value.Year, smallFont, XBrushes.Black, new XRect(280, 105, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("Date Graduated: " + this.student.GraduationDate.Value.ToString("MMMM dd, yyyy"), smallFont, XBrushes.Black, new XRect(280, 120, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("Class Rank: " + this.student.GetClassRank(), smallFont, XBrushes.Black, new XRect(280, 135, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("Program: " + this.student.Program, smallFont, XBrushes.Black, new XRect(280, 150, page1.Width, page1.Height), XStringFormats.TopLeft);
                //Address for the header
                gfx.DrawString("Office of the Registrar, 1350 Regent St, Fredericton, NB", smallFont, XBrushes.Black, new XRect(10, 135, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("E3C 2G6, (506) 458 0653 or 866 619 9900 www.mcft.ca", smallFont, XBrushes.Black, new XRect(10, 150, page1.Width, page1.Height), XStringFormats.TopLeft);

                //Main section of the page
                gfx.DrawRectangle(XPens.Black, new XRect(10, 175, 565, 650));

                gfx.DrawString("YEAR 2 - SEMESTER III", font, XBrushes.Black, new XRect(15, 190, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("SEPTEMBER TO DECEMBER " + (Convert.ToInt32(this.student.GraduationDate.Value.Year) - 1), font, XBrushes.Black, new XRect(15, 200, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("COURSE", font, XBrushes.Black, new XRect(15, 215, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("CLASS GRADE", font, XBrushes.Black, new XRect(270, 215, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("FIELD GRADE", font, XBrushes.Black, new XRect(360, 215, 50, 50), XStringFormats.TopLeft);
                gfx.DrawString("COURSE GRADE", font, XBrushes.Black, new XRect(450, 215, 50, 50), XStringFormats.TopLeft);
                int y = 230;
                foreach (Grade g in this.student.GetGradesForSemester(3))//All the students grades for semester 1
                {
                    gfx.DrawString(g.Subject.Name, smallFont, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                    gfx.DrawString(g.GradeAssigned.ToString(), smallFont, XBrushes.Black, new XRect(450, y, 50, 15), XStringFormats.Center);
                    y += 15;
                }
                y += 10;
                gfx.DrawString("*Indicates grade achieved through supplemental exam", font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString("Semester Average: " + this.student.GetAverageForSemester(3), font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                //Just a theory on how this might work
                string honors = this.student.GetAverageForSemester(1) >= 80 ? "Yes" : "No";
                y += 10;
                gfx.DrawString("Academic Honors: " + honors, font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);

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
                        //In production it wants to put slashes in the reportname for some reason which was causing errors
                        this.reportName = this.reportName.Replace("/","");
                        path = "./Reports/" + this.reportName + ".pdf";
                        string oldPath = "./Reports/Transcript.pdf";
                        System.IO.File.Move(oldPath, path);
                        TranscriptDAL.AddTranscript(path, student);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                return path;
            }
            return path;
        }
    }
}