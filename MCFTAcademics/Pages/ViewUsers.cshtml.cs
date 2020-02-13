using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    [Authorize(Roles = "Admin")]
    public class ViewUsersModel : PageModel
    {
        // XXX: Should refactor out into controller
        public IActionResult OnGetDisable(int id)
        {
            if (User.UserIdMatches(id))
            {
                // don't let people shoot themselves in the foot
                return RedirectToPage("/ViewUsers");
            }
            var user = BL.User.GetUser(id);
            if (user != null)
            {
                user.DisableUser();
            }
            return RedirectToPage("/ViewUsers");
        }

        public void OnGet()
        {

        }
    }
}