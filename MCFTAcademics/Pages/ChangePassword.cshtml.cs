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
        // not required if changing PW of others
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string NewPasswordRepeat { get; set; }
        // for selecting which user to change PW for/presenting that
        [BindProperty]
        [Required]
        [HiddenInput]
        public int TargetUserId { get; set; }

        public bool TargetUserIsSelf() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) == TargetUserId;
        public bool TargetUserIsSelf(int id) => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) == id;
        // should handle names ending in 's'
        public bool AllowedToChangePassword() => TargetUserIsSelf() || User.IsInRole("Admin");

        // XXX: turn into a route with args?
        public IActionResult OnPost()
        {
            // we still perform a bunch of verification in case someone tries to bamboozle us
            if (!AllowedToChangePassword())
            {
                ModelState.AddModelError("", "You aren't allowed to change the password of someone else.");
                return Page();
            }
            bool error = false;
            BL.User user = BL.User.GetUser(TargetUserId);
            if (user == null)
            {
                ModelState.AddModelError("", "The user isn't valid.");
                return Page();
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One of the values is blank.");
                error = true;
            }
            if (TargetUserIsSelf() && !user.ValidatePassword(OldPassword))
            {
                ModelState.AddModelError("", "The current password isn't valid.");
                error = true;
            }
            if (TargetUserIsSelf() && OldPassword == NewPassword)
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

            // success should be a redirect tbh (if self, back to index, otherwise, let them change more passwords)
            // XXX: Sign out?
            return RedirectToPage(TargetUserIsSelf() ? "/Index" : "/ViewUsers");
        }

        void SetViewDataForGet(BL.User targetUser)
        {
            if (targetUser == null)
                ModelState.AddModelError("", "There is no user with that ID.");
            TargetUserId = targetUser?.Id ?? -1;
            ViewData["ChangePasword_TargetUser"] = targetUser;
            ViewData["ChangePassword_IsSelf"] = TargetUserIsSelf();
            ViewData["ChangePassword_TargetDescription"] = TargetUserIsSelf() ? "yourself" : (targetUser?.Name ?? "nobody");
        }

        void SetViewDataForGet() => SetViewDataForGet(BL.User.GetUser(TargetUserId));

        public IActionResult OnGetWithId(int id)
        {
            if (!User.IsInRole("Admin") && !TargetUserIsSelf(id))
            {
                ModelState.AddModelError("", "You aren't allowed to change the password of someone else.");
                SetViewDataForGet(null);
                return Page();
            }
            TargetUserId = id;
            SetViewDataForGet();
            return Page();
        }

        public IActionResult OnGet()
        {
            // always valid, protected by authorization
            TargetUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            SetViewDataForGet();
            return Page();
        }
    }
}