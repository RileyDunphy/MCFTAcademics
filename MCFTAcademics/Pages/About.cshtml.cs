﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "MCFT Academics System";
        }
    }
}
