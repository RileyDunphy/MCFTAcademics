using System;
using System.Collections.Generic;
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
        public Course course { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnGetSelectCourse(int id)
        {
            this.course = course.GetCourseById(id);
            return Page();
        }
    }
}