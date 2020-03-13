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
            document.Info.Title = this.student.FirstName + " " + this.student.LastName + "'s Transcript for " + this.type;
            string path = null;

            // Create an empty page
            PdfPage page1 = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page1);

            // Create a font
            XFont font = new XFont("arial", 11, XFontStyle.Bold);
            XFont smallFont = new XFont("arial", 10);
            XFont reallySmallFont = new XFont("arial", 8);

            GenerateHeader(gfx, font, smallFont, page1);
            //SEMESTER 1 Portion of page if "Year 1" Transcript or "Program" Transcript
            if (this.type == "Program" || this.type == "Year 1" || this.type == "Semester 1")
            {
                GenerateSemesterSection(gfx, font, smallFont, page1, 1, 190);
            }

            if (this.type == "Program" || this.type == "Year 1")
            {
                GenerateSemesterSection(gfx, font, smallFont, page1, 2, 420);
            }
            //Last stuff for Program transcript (only type of transcript that gets a second page)
            if (this.type == "Program")
            {
                //Kind of weird here might improve upon this..
                //Summer practicum is a special course in the database with a semester of 0
                //So it grabs the grade and comment for this part of transcript if this student is enrolled in it
                Grade practicum = Grade.GetSummerPracticum(this.student);
                if (practicum!=null)
                {
                    gfx.DrawString("SUMMER PRACTICUM: " + (practicum.GradeAssigned >= 60 ? "PASS" : "FAIL"), font, XBrushes.Black, new XRect(15, 750, page1.Width, page1.Height), XStringFormats.TopLeft);
                    gfx.DrawString("SUMMER JOB RATING: " + practicum.Comment, font, XBrushes.Black, new XRect(15, 765, page1.Width, page1.Height), XStringFormats.TopLeft);
                }

                gfx.DrawString("Page 1 of 2", reallySmallFont, XBrushes.Black, new XRect(0, 805, page1.Width - 30, page1.Height), XStringFormats.TopRight);

                // Create an empty second page
                PdfPage page2 = document.AddPage();

                // Get an XGraphics object for drawing
                gfx = XGraphics.FromPdfPage(page2);

                GenerateHeader(gfx, font, smallFont, page2);

                GenerateSemesterSection(gfx, font, smallFont, page2, 3, 190);
                int y = GenerateSemesterSection(gfx, font, smallFont, page2, 4, 420);
                y += 10;
                decimal average = this.student.GetAverage();
                gfx.DrawString("Program Average: " + average, font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                y += 10;
                string honors = average >= 80 ? "Yes" : "No";
                gfx.DrawString("Honors: " + honors, font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                GenerateFooter(gfx, font, smallFont, reallySmallFont, page2);
                gfx.DrawString("Page 2 of 2", reallySmallFont, XBrushes.Black, new XRect(0, 805, page1.Width - 30, page1.Height), XStringFormats.TopRight);
            }
            //Couldn't think of a better way to generate the year 2 or individual semester reports then use these IF statements
            if (this.type == "Year 2" || this.type == "Semester 3")
            {
                GenerateSemesterSection(gfx, font, smallFont, page1, 3, 190);
            }
            if (this.type == "Year 2")
            {
                GenerateSemesterSection(gfx, font, smallFont, page1, 4, 420);
            }
            if (this.type == "Semester 2")
            {
                GenerateSemesterSection(gfx, font, smallFont, page1, 2, 190);
            }
            if (this.type == "Semester 4")
            {
                GenerateSemesterSection(gfx, font, smallFont, page1, 4, 190);
            }
            //Put the footer on the first page if it's not the 2-page program transcript
            if (this.type != "Program")
            {
                GenerateFooter(gfx, font, smallFont, reallySmallFont, page1);
                gfx.DrawString("Page 1 of 1", reallySmallFont, XBrushes.Black, new XRect(0, 805, page1.Width - 30, page1.Height), XStringFormats.TopRight);
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
                        this.reportName = this.reportName.Replace("/", "");
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

        public void GenerateHeader(XGraphics gfx, XFont font, XFont smallFont, PdfPage page1)
        {
            // Draw the header at the top
            gfx.DrawString("Generated: "+this.creationDate.ToString("MMMM dd, yyyy"), smallFont, XBrushes.Black, new XRect(10, 0, 300, 100), XStringFormats.CenterLeft);
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
        }

        public int GenerateSemesterSection(XGraphics gfx, XFont font, XFont smallFont, PdfPage page1, int semester, int y)
        {
            int year = semester < 3 ? 1 : 2;
            gfx.DrawString("YEAR " + year + " - SEMESTER " + semester, font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
            y += 10;
            //Find out what months the grades are for
            switch (semester)
            {
                case 1:
                    gfx.DrawString("SEPTEMBER TO DECEMBER " + (Convert.ToInt32(this.student.GraduationDate.Value.Year) - 2), font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                    break;

                case 2:
                    gfx.DrawString("JANUARY TO APRIL " + (Convert.ToInt32(this.student.GraduationDate.Value.Year) - 1), font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                    break;

                case 3:
                    gfx.DrawString("SEPTEMBER TO DECEMBER " + (Convert.ToInt32(this.student.GraduationDate.Value.Year) - 1), font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                    break;

                case 4:
                    gfx.DrawString("JANUARY TO APRIL " + Convert.ToInt32(this.student.GraduationDate.Value.Year), font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                    break;
            }
            y += 15;
            gfx.DrawString("COURSE", font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("CLASS GRADE", font, XBrushes.Black, new XRect(270, y, 50, 50), XStringFormats.TopLeft);
            gfx.DrawString("FIELD GRADE", font, XBrushes.Black, new XRect(360, y, 50, 50), XStringFormats.TopLeft);
            gfx.DrawString("COURSE GRADE", font, XBrushes.Black, new XRect(450, y, 50, 50), XStringFormats.TopLeft);
            y += 15;
            foreach (Grade g in this.student.GetGradesForSemester(semester))//All the students grades for semester 1
            {
                gfx.DrawString(g.Subject.Name, smallFont, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
                if (g.Supplemental)
                {
                    gfx.DrawString(g.GradeAssigned.ToString()+"/60*", smallFont, XBrushes.Black, new XRect(450, y, 50, 15), XStringFormats.Center);
                }
                else
                {
                    gfx.DrawString(g.GradeAssigned.ToString(), smallFont, XBrushes.Black, new XRect(450, y, 50, 15), XStringFormats.Center);
                }
                y += 15;
            }
            y += 10;
            gfx.DrawString("*Indicates grade achieved through supplemental exam", font, XBrushes.Black, new XRect(15, y, page1.Width, page1.Height), XStringFormats.TopLeft);
            decimal average = this.student.GetAverage(semester);
            gfx.DrawString("Semester Average: " + average, font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
            string honors = average >= 80 ? "Yes" : "No";
            y += 10;
            gfx.DrawString("Academic Honors: " + honors, font, XBrushes.Black, new XRect(330, y, page1.Width, page1.Height), XStringFormats.TopLeft);
            return y;
        }

        public void GenerateFooter(XGraphics gfx, XFont font, XFont smallFont, XFont reallySmallFont, PdfPage page1)
        {
            gfx.DrawString("IP - In progress", font, XBrushes.Black, new XRect(15, 715, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("CR - Credit received", font, XBrushes.Black, new XRect(15, 730, page1.Width, page1.Height), XStringFormats.TopLeft);
            if (this.official)
            {
                gfx.DrawString(this.signatureName, smallFont, XBrushes.Black, new XRect(350, 745, 565, 0), XStringFormats.TopLeft);
            }
            gfx.DrawLine(XPens.Black, 345, 760, 570, 760);
            gfx.DrawString("FOR Executive Director, Gerald W. Redmond", smallFont, XBrushes.Black, new XRect(355, 765, 745, 765), XStringFormats.TopLeft);
            gfx.DrawString("Official only if bearing College seal and Executive Directors signature", reallySmallFont, XBrushes.Black, new XRect(0, 795, page1.Width, page1.Height), XStringFormats.TopCenter);
            gfx.DrawString("End of Transcript", font, XBrushes.Black, new XRect(0, 805, page1.Width, page1.Height), XStringFormats.TopCenter);
        }
    }
}