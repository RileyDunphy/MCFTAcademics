using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    public class ChangePasswordModel : PageModel
    {
        // Binding properties for form
        [BindProperty]
        [Required]
        public string OldPassword { get; set; }
        [BindProperty]
        [Required]
        public string NewPassword { get; set; }
        [BindProperty]
        [Required]
        public string NewPasswordRepeat { get; set; }

        public IActionResult OnPost()
        {
            bool error = false;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One of the values is blank.");
                error = true;
            }
            int userId;
            if (!int.TryParse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value, out userId))
            {
                ModelState.AddModelError("", "The user ID couldn't be parsed.");
                // this one is really fatal
                return Page();
            }
            BL.User user = BL.User.GetUser(userId);
            if (!user.ValidatePassword(OldPassword))
            {
                ModelState.AddModelError("", "The current password isn't valid.");
                error = true;
            }
            if (OldPassword == NewPassword)
            {
                ModelState.AddModelError("", "The old and new password are the same.");
                error = true;
            }
            if (NewPassword != NewPasswordRepeat)
            {
                ModelState.AddModelError("", "The new passwords don't match.");
                error = true;
            }
            // TODO: This would be a good place to put password validation rules if we had any.
            // any errors that are pointless do any work with, stop here
            if (error)
                return Page();

            try
            {
                if (user.ChangePassword(NewPassword) != null)
                {
                    ModelState.AddModelError("", "The password couldn't be changed.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("",
                    "There was an exception from the system changing the password;" +
                    "report this to an administrator: " + e.Message);
            }

            // success should be a redirect tbh
            // XXX: Sign out?
            return RedirectToPage("/Index");
        }

        public void OnGet()
        {

        }
    }
}