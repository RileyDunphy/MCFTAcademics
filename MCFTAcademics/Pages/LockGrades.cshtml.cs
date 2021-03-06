﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCFTAcademics.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    // Actually academic or executive chair, but...
    [Authorize(Roles = "Admin")]
    public class LockGradesModel : PageModel
    {
        public void OnGet()
        {

        }
        public ActionResult OnGetAjax(int studentId, int courseId, DateTime? unlockedUntil = null)
        {
            bool result = Grade.ToggleGradeLock(studentId, courseId,unlockedUntil);
            return new JsonResult(result);
        }
    }
}