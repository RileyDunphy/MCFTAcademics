using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Username { get; set; }
        [BindProperty]
        [Required]
        public string RealName { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string NewPasswordRepeat { get; set; }

        public IActionResult OnPost()
        {
            bool error = false;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One of the values is blank.");
                error = true;
            }
            if (BL.User.GetUser(Username) != null)
            {
                ModelState.AddModelError("", "A user with that username exists already.");
                error = true;
            }
            if (NewPassword != NewPasswordRepeat)
            {
                ModelState.AddModelError("", "The new passwords don't match.");
                error = true;
            }
            if (error)
                return Page();

            BL.User newUser = null;
            try
            {
                newUser = BL.User.CreateUser(RealName, Username, NewPassword);
                if (newUser == null)
                {
                    ModelState.AddModelError("", "The user couldn't be created.");
                    error = true;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("",
                    "There was an exception from the system creating the user;" +
                    "report this to an administrator: " + e.Message);
                error = true;
            }

            if (error)
                return Page();
            // safe to use ID
            return RedirectToPage("/ViewRoles", "WithId", new { id = newUser.Id });
        }

        public void OnGet()
        {

        }
    }
}