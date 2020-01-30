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
        [BindProperty]
        public CourseCode courseCode { get; set; }

        public async Task<IActionResult> Create()
        {
            Console.WriteLine(course.Name);
            return Page();
        }
        public void OnGet()
        {

        }
        public IActionResult OnGetSelectCourse(int id)
        {
            this.course = Course.getCourseById(id);
            this.courseCode = CourseCode.getNewestCourseCodeById(id);
            return Page();
        }
        [HttpPost]
        public IActionResult Delete(int prereq, int id)
        {
            this.course.Prerequisites.RemoveAt(prereq);
            this.course = Course.getCourseById(id);
            this.courseCode = CourseCode.getNewestCourseCodeById(id);
            return RedirectToAction("OnGetSelectCourse(id)");
        }
    }
}