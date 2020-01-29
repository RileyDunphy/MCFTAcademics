using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;
using MCFTAcademics.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MCFTAcademics
{
    public class LoginModel : PageModel
    {
        //A lot of this code came from this tutorial: 
        //http://future-shock.net/blog/post/creating-a-simple-login-in-asp.net-core-2-using-authentication-and-authorization-not-identity
        [BindProperty]
        public LoginData loginData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // the Required parts on the model will automatically print for us
                return Page(); // no point in logging in then
            }
            BL.User user = null;
            try
            {
                user = BL.User.GetUser(loginData.Username);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("",
                    "There was an exception from the system getting the user info;" +
                    "report this to an administrator: " + e.Message);
                return Page();
            }
            if (user == null)
            {
                ModelState.AddModelError("", "There is no user with that username.");
                return Page();
            }
            if (!user.ValidatePassword(loginData.Password))
            {
                ModelState.AddModelError("", "The password doesn't match.");
                return Page();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            if (!string.IsNullOrWhiteSpace(user.Name))
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            var roles = user.GetRoles();
            if (roles != null && roles.Count() > 0)
            {
                identity.AddClaims(roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));
            }

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = loginData.RememberMe });
            return RedirectToPage("Index");
        }

        public class LoginData
        {
            [Required]
            public string Username { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}