using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    // based on changepassword
    public class ChangeProfileModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Username { get; set; }
        [BindProperty]
        [Required]
        public string RealName { get; set; }
        [BindProperty]
        [Required]
        [HiddenInput]
        public int TargetUserId { get; set; }

        // only useful for this page
        public bool AllowedToChangeProfile() => User.UserIdMatches(TargetUserId) || User.IsInRole("Admin");

        public IActionResult OnPost()
        {
            // we still perform a bunch of verification in case someone tries to bamboozle us
            if (!AllowedToChangeProfile())
            {
                ModelState.AddModelError("", "You aren't allowed to change the profile of someone else.");
                SetViewDataForGet();
                return Page();
            }
            BL.User user = BL.User.GetUser(TargetUserId);
            if (user == null)
            {
                ModelState.AddModelError("", "The user isn't valid.");
                SetViewDataForGet();
                return Page();
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One of the values is blank.");
                SetViewDataForGet();
                return Page();
            }
            if (user.Username != Username && BL.User.GetUser(Username) != null)
            {
                ModelState.AddModelError("", "A user with that username exists already.");
                SetViewDataForGet();
                return Page();
            }

            try
            {
                if (user.ChangeProfile(RealName, Username) != null)
                {
                    ModelState.AddModelError("", "The profile couldn't be changed.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("",
                    "There was an exception from the system changing the profile;" +
                    "report this to an administrator: " + e.Message);
            }

            // success should be a redirect tbh (if self, back to index, otherwise, let them change more passwords)
            // XXX: Sign out?
            return RedirectToPage(User.UserIdMatches(TargetUserId) ? "/Index" : "/ViewUsers");
        }

        void SetViewDataForGet(BL.User targetUser)
        {
            if (targetUser == null)
                ModelState.AddModelError("", "There is no user with that ID.");
            TargetUserId = targetUser?.Id ?? -1;
            RealName = targetUser?.Name;
            Username = targetUser?.Username;
            ViewData["ChangeProfile_TargetUser"] = targetUser;
            ViewData["ChangeProfile_IsSelf"] = User.UserIdMatches(TargetUserId);
            ViewData["ChangeProfile_TargetDescription"] = targetUser.GetReferentialName(User);
        }

        void SetViewDataForGet() => SetViewDataForGet(BL.User.GetUser(TargetUserId));

        public IActionResult OnGetWithId(int id)
        {
            if (!AllowedToChangeProfile())
            {
                ModelState.AddModelError("", "You aren't allowed to change the profile of someone else.");
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