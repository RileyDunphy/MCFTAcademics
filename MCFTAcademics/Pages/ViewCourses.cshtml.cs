using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class ViewCoursesModel : PageModel
    {
        [Authorize(Roles = "Admin,Instructor")]
        public void OnGet()
        {
            ViewData["Title"] = "Courses you are instructing";
            var courses = BL.Course.GetCoursesByInstructor(User.IdAsInt());
            ViewData["ViewData_Courses"] = courses;
        }

        // See its counterpart in ViewGrades
        [Authorize(Roles = "Admin,Instructor")]
        public void OnGetAll()
        {
            ViewData["Title"] = "All Courses";
            var courses = BL.Course.GetAllCourses();
            ViewData["ViewData_Courses"] = courses;
        }
    }
}