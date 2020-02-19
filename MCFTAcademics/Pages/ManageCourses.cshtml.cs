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
        public string alertMessage { get; set; }

        //Variable to store old course code, incase it is changed. 
        //If it is changed, create a new entry in course code table
        public static string code { get; set; }

        //Flag to tell whether to add course or update course on submit, or if null then no menu item selected
        public static bool? add { get; set; }

        // XXX: Ugly
        // Used for persisting the list of courses between states.
        public IEnumerable<Course> ShownCourses { get; set; }
        // Used for reloading the page. Each new page load needs to reload the
        // list of courses due to the stateless nature of HTTP.
        [BindProperty]
        [HiddenInput]
        public bool ShowAllCourses { get; set; }

        public IActionResult OnPost()
        {
            try
            {
                // in case we have to be on the same page again?
                Init();
                RefreshShownCourses();
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
                Staff leadStaff = new Staff(Convert.ToInt32(Request.Form["leadStaff"]), "", "lead");
                Staff supportStaff = null;
                //Leave support staff as null unless there was a choice selected (its optional)
                if (Request.Form["supportStaff"] != "")
                {
                    supportStaff = new Staff(Convert.ToInt32(Request.Form["supportStaff"]), "", "support");
                }
                List<Prerequisite> prereqs = new List<Prerequisite>();
                for (int i = 0; i < Convert.ToInt32(Request.Form["count"]); i++)
                {
                    Prerequisite prereq = null;
                    //Validation to see if the entered prereq coursecode is valid
                    //If it isn't, the method will return 0 and then it will refresh with a message (should be no courseid of 0)
                    int prereqId = CourseCode.GetIdByCourseCode(Request.Form["prereqCode+" + i]);
                    if (prereqId == 0)
                    {
                        this.dropdownText = "Please select a course to change";
                        this.alertMessage = "You entered a invalid Course Code for a Prerequisite";
                        return Page();
                    }
                    //If it returned a valid id, now check if its a coreq or a prereq
                    if (Request.Form["reqRadio+" + i].Equals("prereq"))
                    {
                        prereq = new Prerequisite(id, prereqId, true, false);
                    }
                    else if (Request.Form["reqRadio+" + i].Equals("coreq"))
                    {
                        prereq = new Prerequisite(id, prereqId, false, true);
                    }
                    prereqs.Add(prereq);
                }
                if (add == false)
                {
                    Course c = new Course(id, name, credit, description, lectureHours, labHours, examHours, totalHours, revisionNumber, program, accreditation, prereqs, leadStaff, supportStaff);
                    c.UpdateCourse();
                }
                else if (add == true)
                {
                    id = Course.AddCourse(new Course(id, name, credit, description, lectureHours, labHours, examHours, totalHours, revisionNumber, program, accreditation, prereqs, leadStaff, supportStaff));
                }
                if (courseCode != code)
                {
                    CourseCode.AddCourseCode(id, new CourseCode(courseCode, startDate, endDate, semester));
                }
                return ShowAllCourses ? RedirectToPage("ManageCourses", "all") : RedirectToPage("ManageCourses");
            }
            catch (Exception ex)
            {
                this.dropdownText = "Please select a course to change";
                this.alertMessage = "An error occured";
                return Page();
            }
        }

        void RefreshShownCourses()
        {
            // XXX: Check if the user is allowed to do this.
            if (ShowAllCourses)
            {
                ViewData["Title"] = "Manage courses";
                var courses = Course.GetAllCourses();
                ShownCourses = courses;
            }
            else
            {
                ViewData["Title"] = "Manage courses you are instructing";
                var courses = Course.GetCoursesByInstructor(User.IdAsInt());
                ShownCourses = courses;
            }
        }

        void Init()
        {
            this.alertMessage = "";
            this.dropdownText = "Please select a course to change";
        }

        public void OnGet()
        {
            ShowAllCourses = false;
            RefreshShownCourses();
            Init();
        }

        // consistent, but awkward since we refresh a lot
        public void OnGetAll()
        {
            ShowAllCourses = true;
            RefreshShownCourses();
            Init();
        }

        public IActionResult OnGetSelectCourse(int id, bool forAll)
        {
            // XXX: We could consider AJAX. Refresh will handle if all needs auth.
            ShowAllCourses = forAll;
            RefreshShownCourses();
            // XXX: We don't check if the user is allowed to use the course
            this.course = Course.GetCourseById(id);
            this.courseCode = CourseCode.GetNewestCourseCodeById(id);
            code = courseCode.Code;
            add = false;
            this.dropdownText = this.course.Name;
            return Page();
        }

        public IActionResult OnGetAddCourse(int id, bool forAll)
        {
            // XXX: We could consider AJAX. Refresh will handle if all needs auth.
            ShowAllCourses = forAll;
            RefreshShownCourses();
            // XXX: We don't check if the user is allowed to use the course
            this.course = new Course();
            this.courseCode = new CourseCode();
            code = courseCode.Code;
            add = true;
            this.dropdownText = "Add new course";
            return Page();
        }
    }
}