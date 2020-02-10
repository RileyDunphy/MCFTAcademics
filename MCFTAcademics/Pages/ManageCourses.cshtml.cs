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
    public class ManageCoursesModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter a course")]
        [Display(Name = "Course")]
        public Course course { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please enter a course code")]
        [Display(Name = "Course Code")]
        public CourseCode courseCode { get; set; }
        [BindProperty]
        public string dropdownText { get; set; }

        //Variable to store old course code, incase it is changed. 
        //If it is changed, create a new entry in course code table
        public static string code { get; set; }

        //Flag to tell whether to add course or update course on submit, or if null then no menu item selected
        public static bool? add { get; set; }

        public IActionResult OnPost()
        {
            try
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
                string program = Request.Form["program"];
                bool accreditation = Convert.ToBoolean(Request.Form["accreditation"]);
                int semester = Convert.ToInt32(Request.Form["semester"]);
                DateTime startDate = Convert.ToDateTime(Request.Form["startDate"]);
                DateTime endDate = Convert.ToDateTime(Request.Form["endDate"]);

                List<Prerequisite> prereqs = new List<Prerequisite>();
                for (int i = 0; i < Convert.ToInt32(Request.Form["count"]); i++)
                {
                    Prerequisite prereq = null;
                    if (Request.Form["reqRadio+" + i].Equals("prereq"))
                    {
                        prereq = new Prerequisite(id, CourseCode.GetIdByCourseCode(Request.Form["prereqCode+" + i]), true, false);
                    }
                    else if (Request.Form["reqRadio+" + i].Equals("coreq"))
                    {
                        prereq = new Prerequisite(id, CourseCode.GetIdByCourseCode(Request.Form["prereqCode+" + i]), false, true);
                    }
                    prereqs.Add(prereq);
                }
                if (add == false)
                {
                    Course c = new Course(id, name, credit, description, lectureHours, labHours, examHours, totalHours, revisionNumber, program, accreditation, prereqs);
                    c.UpdateCourse();
                }
                else if (add == true)
                {
                    id = Course.AddCourse(new Course(id, name, credit, description, lectureHours, labHours, examHours, totalHours, revisionNumber, program, accreditation, prereqs));
                }
                if (courseCode != code)
                {
                    CourseCode.AddCourseCode(id, this.courseCode);
                }
                return RedirectToPage("ManageAllCourses");
            }
            catch (Exception ex)
            {
                this.dropdownText = "Please select a course to change";
                return RedirectToPage("ManageAllCourses");
            }
        }
        public void OnGet()
        {
            this.dropdownText = "Please select a course to change";
        }
        public IActionResult OnGetSelectCourse(int id)
        {
            this.course = Course.GetCourseById(id);
            this.courseCode = CourseCode.GetNewestCourseCodeById(id);
            code = courseCode.Code;
            add = false;
            this.dropdownText = this.course.Name;
            return Page();
        }

        public IActionResult OnGetAddCourse(int id)
        {
            this.course = new Course();
            this.courseCode = new CourseCode();
            code = courseCode.Code;
            add = true;
            this.dropdownText = "Add new course";
            return Page();
        }
    }
}