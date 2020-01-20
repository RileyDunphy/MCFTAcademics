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
            if (ModelState.IsValid)
            {
                var isValid = loginData.Login();
                if (!isValid)
                {
                    ModelState.AddModelError("", "username or password is invalid");
                    return Page();
                }
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginData.Username));
                identity.AddClaim(new Claim(ClaimTypes.Name, loginData.Username));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = loginData.RememberMe });
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError("", "username or password is blank");
                return Page();
            }
        }

        public class LoginData
        {
            [Required]
            public string Username { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }

            public bool Login()
            {
                bool loggedIn;
                string sql = "select username, password from [mcftacademics].[dbo].[Users] where username = @username AND password = @password";
                SqlConnection conn = DbConn.GetConnection();
                SqlCommand selectCommand = new SqlCommand(sql, conn);
                selectCommand.Parameters.AddWithValue("@username", this.Username);
                selectCommand.Parameters.AddWithValue("@password", this.Password);
                try
                {
                    conn.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        loggedIn= true;
                    }
                    else
                    {
                        loggedIn= false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    loggedIn= false;
                }
                finally
                {//close the connection either way
                    conn.Close();
                }
                return loggedIn;
            }
        }
    }
}