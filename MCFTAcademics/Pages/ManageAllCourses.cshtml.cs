using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class ManageAllCoursesModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter a course")]
        [Display(Name = "Course")]
        public Course course { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please enter a course")]
        [Display(Name = "Course")]
        public CourseCode courseCode { get; set; }
        public static string code { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            int id = Convert.ToInt32(Request.Form["courseId"]);
            string name = Request.Form["courseTitle"];
            string courseCode = Request.Form["courseCode"];
            string description = Request.Form["description"];
            int credit = Convert.ToInt32(Request.Form["credit"]);
            int lectureHours = Convert.ToInt32(Request.Form["lectureHours"]);
            int labHours = Convert.ToInt32(Request.Form["labHours"]);
            int totalHours = Convert.ToInt32(Request.Form["totalHours"]);
            int examHours = Convert.ToInt32(Request.Form["examHours"]);
            decimal revisionNumber = Convert.ToDecimal(Request.Form["revisionNumber"]);
            Course.updateCourse(new Course(id,name,credit,DateTime.Now,DateTime.Now,description,lectureHours,labHours,examHours,totalHours, revisionNumber, null));
            if (courseCode != code)
            {
                CourseCode.addCourseCode(id, courseCode);
            }
            return RedirectToPage("ManageAllCourses");
        }
        public void OnGet()
        {

        }
        public IActionResult OnGetSelectCourse(int id)
        {
            this.course = Course.getCourseById(id);
            this.courseCode = CourseCode.getNewestCourseCodeById(id);
            code = courseCode.Code;
            return Page();
        }

        public IActionResult OnGetAddCourse(int id)
        {
            this.course = Course.getCourseById(id);
            this.courseCode = CourseCode.getNewestCourseCodeById(id);
            code = courseCode.Code;
            return Page();
        }
    }
}