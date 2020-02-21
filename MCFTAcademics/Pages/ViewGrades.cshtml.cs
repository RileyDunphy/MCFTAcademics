using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class ViewGradesModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Instructor Grades";
            var courses = BL.Course.GetCoursesByInstructor(User.IdAsInt());
            ViewData["ViewGrades_Courses"] = courses;
        }

        /* XXX: What role does this belong to? */
        public void OnGetAll()
        {
            ViewData["Title"] = "All Grades";
            var courses = BL.Course.GetAllCourses();
            ViewData["ViewGrades_Courses"] = courses;
        }
    }
}