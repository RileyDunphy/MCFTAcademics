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
        [BindProperty]
        [Required(ErrorMessage = "Please enter a StudentId")]
        [Display(Name = "Student")]
        public Student s { get; set; }


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
        {   try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                int id = Convert.ToInt32(Request.Form["studentId"]);
                this.s = null;
                this.s = Student.GetStudentByStudentId(id);
                //List<Grade> grades=StudentDAL.GetGradeByStudentId(id);
                IEnumerable<Grade> grades = s.GetGrades();

                foreach (Grade g in grades) {
                    Console.WriteLine(g.ToString());
                }

            }
            catch (Exception ex) { }
            return Page();
        }
        public ActionResult OnGetAjax(int grade, int studentId, string comment, int courseId)
        {   //almost empty course object
            Course c=new Course(courseId,"",1,1,"",1,1,1,1,12,"",true);

            Grade update = new Grade(grade,DateTime.Now,false,0m,false,c,comment);

            bool response=Grade.UpdateGrade(update, studentId);

            return new JsonResult(response);
        }

    }


            
        
}
