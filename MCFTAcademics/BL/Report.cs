using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.BL
{/// <summary>
/// This class will be used to generate academic reports for the college.
/// </summary>
    public class Report
    {
        private TimeSpan reportSpan;
        private string program;
        private int semester;
        private int year;
        private string reportName="someReport";

        public Report(string program, int semester, int year) 
        {
            this.program = program;
            this.semester = semester;
            this.year = year;
        }

        public string generateReport() {
            
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

            //SEMESTER 1 Portion of page if "Year 1" Transcript or "Program" Transcript
            if (this.semester == 1)
            {
                gfx.DrawString("SEMESTER 1 ", font, XBrushes.Black, new XRect(15, 200, page1.Width, page1.Height), XStringFormats.Center);
            }

                

                // Create an empty second page
                PdfPage page2 = document.AddPage();

                // Get an XGraphics object for drawing
                gfx = XGraphics.FromPdfPage(page2);

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
