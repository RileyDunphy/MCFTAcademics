using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class LockGradesModel : PageModel
    {
        public void OnGet()
        {

        }
        public ActionResult OnGetAjax(int studentId, int courseId)
        {
            bool result = Grade.ToggleGradeLock(studentId, courseId);
            return new JsonResult(result);
        }
    }
}