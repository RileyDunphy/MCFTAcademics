using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
            // XXX: If the user's roles updated, we should offer to re-update them, or at least invalidate their session.
        }
    }
}