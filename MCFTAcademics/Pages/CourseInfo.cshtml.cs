using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class CourseInfoModel : PageModel
    {
        public void OnGetInfo(int id)
        {
            var course = BL.Course.GetCourseById(id);
            ViewData["Title"] = course.Name;
            ViewData["ViewData_Course"] = course;
        }
    }
}