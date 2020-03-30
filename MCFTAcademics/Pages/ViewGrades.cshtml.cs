using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class ViewGradesModel : PageModel
    {
        [Authorize(Roles = "Admin,Instructor")]
        public void OnGet()
        {
            ViewData["Title"] = "Instructor Grades";
            var courses = BL.Course.GetCoursesByInstructor(User.IdAsInt());
            ViewData["ViewGrades_Courses"] = courses;
        }

        // Each instructor has unlimited view permissions to see student grades in other courses.
        [Authorize(Roles = "Admin,Instructor")]
        public void OnGetAll()
        {
            ViewData["Title"] = "All Grades";
            var courses = BL.Course.GetAllCourses();
            ViewData["ViewGrades_Courses"] = courses;
        }
    }
}