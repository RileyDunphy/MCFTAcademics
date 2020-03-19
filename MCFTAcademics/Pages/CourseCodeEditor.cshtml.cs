using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class CourseCodeEditorModel : PageModel
    {
        [BindProperty]
        public Course course { get; set; }
        public string alertMessage { get; set; }
        public void OnGet(int id)
        {
            this.course = BL.Course.GetCourseById(id);
            ViewData["Title"] = course.Name;
            ViewData["ViewData_Course"] = course;
        }

        public IActionResult OnPost()
        {
            try
            {
                List<CourseCode> coursecodes = new List<CourseCode>();
                for (int i = 0; i < Convert.ToInt32(Request.Form["count"]); i++)
                {
                    //Still not sure if the semester changes per course code or if thats a course dependent thing
                    CourseCode coursecode =  new CourseCode(Request.Form["txtCode+" + i], Convert.ToDateTime(Request.Form["txtStartDate+" + i]), Convert.ToDateTime(Request.Form["txtEndDate+" + i]), Convert.ToInt32(Request.Form["semester+"+i]));

                    coursecodes.Add(coursecode);
                }
                bool result = CourseCode.UpdateCourseCodes(coursecodes, Convert.ToInt32(Request.Form["courseId"]));
                if (result)
                {
                    this.alertMessage = "Course Codes have been updated";
                    return RedirectToPage("CourseCodeEditor", new { id = Convert.ToInt32(Request.Form["courseId"]) });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return FailWithMessage("There was an exception from the system updating the course codes;" +
                    "report this to an administrator: " + ex.Message);
            }
        }

        IActionResult FailWithMessage(string errorMessage)
        {
            // everything should be blanked out here
            this.alertMessage = errorMessage;
            return Page();
        }
    }
}