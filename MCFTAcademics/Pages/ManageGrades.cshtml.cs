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
using Microsoft.AspNetCore.Authorization;

namespace MCFTAcademics
{
    public class ManageGrades : PageModel
    {
        public IEnumerable<Grade> Grades { get; set; }
        // ugly because we'd get a a lot of the same student otherwise
        public Dictionary<int, Student> StudentMapping { get; set; }
        public bool ShowStudentIdForm { get; set; }

        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult OnGet()
        {
            ViewData["Title"] = "Grades";
            ShowStudentIdForm = false;
            return Page();
        }

        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult OnGetByStudent()
        {
            ViewData["Title"] = "Grades by Student";
            ShowStudentIdForm = true;
            return Page();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult OnPostByStudent(string studentCode)
        {
            ViewData["Title"] = "Grades by Student";
            ShowStudentIdForm = true;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var student = Student.GetStudent(studentCode);
                if (student == null)
                {
                    ModelState.AddModelError("",
                        "There was no student with that ID.");
                    return Page();
                }
                Grades = student.GetGrades().OrderBy(Grade => Grade);
                StudentMapping = new Dictionary<int, Student>
                {
                    { student.Id, student }
                };
            }
            catch (Exception ex)
            {
                //TODO: add logging for errors
                ModelState.AddModelError("",
                    "There was an exception from the system getting the grades;" +
                    "report this to an administrator: " + ex.Message);
            }
            return Page();
        }

        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult OnGetByCurrentInstructor()
        {
            ViewData["Title"] = "Grades by Courses You Teach";
            ShowStudentIdForm = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                Grades = Grade.GetGradesForInstructor(
                    new BL.User(null, null, null, User.IdAsInt()));
                StudentMapping = Grades
                    .Select(grade => grade.StudentId)
                    .Distinct()
                    .Select(studentId => Student.GetStudent(studentId))
                    .ToDictionary(student => student.Id);
            }
            catch (Exception ex)
            {
                //TODO: add logging for errors
                ModelState.AddModelError("",
                    "There was an exception from the system getting the grades;" +
                    "report this to an administrator: " + ex.Message);
            }
            return Page();
        }

        [Authorize(Roles = "Admin,Instructor")]
        public ActionResult OnGetAjax(int grade, int studentId, string comment, int courseId, bool isSupplemental)
        {
            if (grade > 100)
                return new JsonResult(new { Error = "The grade is over 100." })
                {
                    StatusCode = 400
                };
            if (grade < 0)
                return new JsonResult(new { Error = "The grade is below 0." })
                {
                    StatusCode = 400
                };

            var course = Course.GetCourseById(courseId);
            // should supporting staff be allowed to change grades?
            if (!(User.IsInRole("Admin") || course.GetLeadStaff().UserId == User.IdAsInt()))
                return new JsonResult(new { Error = "You aren't an admin or lead instructor for this course." })
                {
                    StatusCode = 400
                };
            // now check if the grade is locked (we should have a more direct way to get a specific grade, this is likely slow)
            var oldGrade = Grade.GetAllGrades()
                .Where(x => x.StudentId == studentId
                    && x.Subject.Id == courseId
                    /* XXX: More criteria? */)
                .FirstOrDefault();
            if (oldGrade == null)
                return new JsonResult(new { Error = "The grade doesn't exist." })
                {
                    StatusCode = 400
                };
            if (oldGrade.Locked)
                return new JsonResult(new { Error = "The grade is locked." })
                {
                    StatusCode = 400
                };

            Grade update = new Grade(studentId,grade,DateTime.Now,false,0m,isSupplemental,course,comment,null);

            bool response=Grade.UpdateGrade(update, studentId);

             return new JsonResult(response);
        }

    }


            
        
}
