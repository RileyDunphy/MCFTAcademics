using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Authorization;
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

        //Flag to tell whether to add course or update course on submit, or if null then no menu item selected
        [BindProperty]
        [HiddenInput]
        public bool? Add { get; set; }

        // XXX: Ugly
        // Used for persisting the list of courses between states.
        public IEnumerable<Course> ShownCourses { get; set; }
        // Used for reloading the page. Each new page load needs to reload the
        // list of courses due to the stateless nature of HTTP.
        [BindProperty]
        [HiddenInput]
        public bool ShowAllCourses { get; set; }

        bool AllowedToModify(int oldCourseId)
        {
            var isAdmin = User.IsInRole("Admin");
            // Admins can always modify/add.
            if (isAdmin)
                return true;
            // If adding (assume if somehow not set)
            if ((Add ?? true) && !isAdmin)
                return false;

            // Finally, if we're not adding or admins, only if we're the lead staff
            var oldCourse = Course.GetCourseById(oldCourseId);
            return (oldCourse != null) && (User.UserIdMatches(oldCourse.GetLeadStaff().UserId));
        }

        [Authorize(Roles = "Admin,Instructor")]
        IActionResult FailWithMessage(string errorMessage)
        {
            // everything should be blanked out here
            this.dropdownText = "Please select a course to change";
            this.alertMessage = errorMessage;
            return Page();
        }

        [Authorize(Roles = "Admin,Instructor")]
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

                if (!int.TryParse(Request.Form["courseId"], out int id))
                {
                    return FailWithMessage("The ID is invalid.");
                }

                // we have enough information for access control
                if (!AllowedToModify(id))
                {
                    return FailWithMessage("You aren't allowed to change this course.");
                }

                string name = Request.Form["courseTitle"];
                string courseCode = Request.Form["courseCode"];
                string description = Request.Form["description"];
                if (!int.TryParse(Request.Form["credit"], out int credit)
                    || credit < 0)
                {
                    return FailWithMessage("The credit is invalid.");
                }
                if (!int.TryParse(Request.Form["lectureHours"], out int lectureHours)
                    || lectureHours < 0)
                {
                    return FailWithMessage("The lecture hours are invalid.");
                }
                if (!int.TryParse(Request.Form["labHours"], out int labHours)
                    || labHours < 0)
                {
                    return FailWithMessage("The lab hours are invalid.");
                }
                if (!int.TryParse(Request.Form["totalHours"], out int totalHours)
                    || totalHours < 0)
                {
                    return FailWithMessage("The total hours are invalid.");
                }
                if (!int.TryParse(Request.Form["examHours"], out int examHours)
                    || examHours < 0)
                {
                    return FailWithMessage("The exam hours are invalid.");
                }
                if (!decimal.TryParse(Request.Form["revisionNumber"], out decimal revisionNumber))
                {
                    return FailWithMessage("The revision number is invalid.");
                }
                string program = Request.Form["program"];
                if (!bool.TryParse(Request.Form["accreditation"], out bool accreditation))
                {
                    return FailWithMessage("The accreditation is invalid.");
                }
                if (!int.TryParse(Request.Form["semester"], out int semester)
                    || semester < 0)
                {
                    return FailWithMessage("The semester is invalid.");
                }
                if (!DateTime.TryParse(Request.Form["startDate"], out DateTime startDate))
                {
                    return FailWithMessage("The start date is invalid.");
                }
                if (!DateTime.TryParse(Request.Form["endDate"], out DateTime endDate))
                {
                    return FailWithMessage("The end date is invalid.");
                }
                if (startDate > endDate)
                {
                    return FailWithMessage("The end of the course is before when it starts.");
                }
                if (!int.TryParse(Request.Form["leadStaff"], out int leadStaffId)
                    || leadStaffId < 0)
                {
                    return FailWithMessage("The lead staff ID is invalid.");
                }
                Staff leadStaff = new Staff(leadStaffId, "", "lead");

                Staff supportStaff = null;
                //Leave support staff as null unless there was a choice selected (its optional)
                if (!string.IsNullOrWhiteSpace(Request.Form["supportStaff"]))
                {
                    if (!int.TryParse(Request.Form["supportStaff"], out int supportStaffId)
                        || supportStaffId < 0)
                    {
                        return FailWithMessage("The support staff ID is invalid.");
                    }
                    supportStaff = new Staff(supportStaffId, "", "support");
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
                        return FailWithMessage("You entered a invalid Course Code for a Prerequisite");
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
                if (Add == false)
                {
                    Course c = new Course(id, name, credit, description, lectureHours, labHours, examHours, totalHours, revisionNumber, program, accreditation);
                    c.UpdateCourse(leadStaff, supportStaff, prereqs);
                }
                else if (Add == true)
                {
                    id = Course.AddCourse(new Course(id, name, credit, description, lectureHours, labHours, examHours, totalHours, revisionNumber, program, accreditation), leadStaff, supportStaff, prereqs);
                }
                // XXX: really flawed and prob won't be performant, we need proper course code management
                var oldCourseCode = !(Add ?? false) ? Course.GetCourseById(id)?.GetCourseCode()?.Code : null;
                if (courseCode != oldCourseCode)
                {
                    CourseCode.AddCourseCode(id, new CourseCode(courseCode, startDate, endDate, semester));
                }
                return ShowAllCourses ? RedirectToPage("ManageCourses", "all") : RedirectToPage("ManageCourses");
            }
            catch (Exception ex)
            {
                return FailWithMessage("There was an exception from the system updating the course;" +
                    "report this to an administrator: " + ex.Message);
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
        [Authorize(Roles = "Admin")]
        public void OnGetAll()
        {
            ShowAllCourses = true;
            RefreshShownCourses();
            Init();
        }

        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult OnGetSelectCourse(int id, bool forAll)
        {
            // XXX: We could consider AJAX. Refresh will handle if all needs auth.
            ShowAllCourses = forAll && User.IsInRole("Admin");
            RefreshShownCourses();
            // XXX: We don't check if the user is allowed to use the course
            this.course = Course.GetCourseById(id);
            this.courseCode = course.GetCourseCode();
            Add = false;
            this.dropdownText = this.course.Name;
            return Page();
        }

        [Authorize( Roles = "Admin" )]
        public IActionResult OnGetAddCourse(int id, bool forAll)
        {
            // XXX: We could consider AJAX. Refresh will handle if all needs auth.
            ShowAllCourses = forAll && User.IsInRole("Admin");
            RefreshShownCourses();
            // XXX: We don't check if the user is allowed to use the course
            this.course = new Course();
            Add = true;
            this.dropdownText = "Add new course";
            return Page();
        }

        [Authorize(Roles = "Admin,Instructor")]
        public ActionResult OnGetAjax(string code)
        {
            if (code != null)
            {
                List<string> prereqCodes = CourseCode.SearchCourseCodes(code);
                JsonResult result = new JsonResult(prereqCodes);
                return result;
            }
            return new JsonResult("");
        }

        [Authorize(Roles = "Admin,Instructor")]
        public ActionResult OnGetCheckPrereqCode(string code)
        {
            if (code != null)
            {
                if (CourseCode.GetIdByCourseCode(code) == 0)
                {
                    return new JsonResult(false);
                }
                else
                {
                    return new JsonResult(true);
                }
            }
            return new JsonResult(false);
        }
    }
}