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
            var isValid = loginData.Login();
            switch (isValid)
            {
                case LoginStatus.NoUser:
                    ModelState.AddModelError("", "There is no user with that username.");
                    return Page(); // don't actually add the identity then
                case LoginStatus.DatabaseFailure:
                    ModelState.AddModelError("", "There was an error connecting to the database.");
                    return Page();
                case LoginStatus.IdFailure:
                    ModelState.AddModelError("", "There was an error getting your user ID.");
                    return Page();
                case LoginStatus.InvalidPassword:
                    ModelState.AddModelError("", "The password doesn't match.");
                    return Page();
                case LoginStatus.Success:
                    break;
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginData.Id.ToString(), ClaimValueTypes.Integer));
            identity.AddClaim(new Claim(ClaimTypes.Name, loginData.Username));
            if (!string.IsNullOrWhiteSpace(loginData.RealName))
                identity.AddClaim(new Claim(ClaimTypes.GivenName, loginData.RealName));
            var roles = loginData.GetRolesFromDatabase();
            if (roles != null)
            {
                identity.AddClaims(roles.Select(x => new Claim(ClaimTypes.Role, x)));
            }

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = loginData.RememberMe });
            return RedirectToPage("Index");
        }

        public enum LoginStatus
        {
            Success, NoUser, InvalidPassword, IdFailure, DatabaseFailure
        }

        public class LoginData
        {
            [Required]
            public string Username { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }

            public int Id { get; protected set; }

            public string RealName { get; protected set; }

            // XXX: Should be moved to proper user handling
            int GetUserId()
            {
                var connection = DbConn.GetConnection();
                connection.Open();
                // XXX: use a UDF
                var command = new SqlCommand("[mcftacademics].dbo.Get_UserId", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", Username);
                try
                {
                    var res = (int)command.ExecuteScalar();
                    connection.Close();
                    return res;
                }
                catch (SqlException e)
                {
                    // XXX: log exception
                    connection.Close();
                    return -1;
                }
            }

            string GetUserRealName()
            {
                var connection = DbConn.GetConnection();
                connection.Open();
                // XXX: use a UDF
                var command = new SqlCommand("[mcftacademics].dbo.Get_RealName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userIdentity", Id);
                try
                {
                    var res = command.ExecuteScalar().ToString();
                    connection.Close();
                    return res;
                }
                catch (SqlException e)
                {
                    // XXX: log exception
                    connection.Close();
                    return null;
                }
            }

            // really simplistic too
            internal IEnumerable<string> GetRolesFromDatabase()
            {
                var connection = DbConn.GetConnection();
                connection.Open();
                // XXX: use a UDF
                var command = new SqlCommand("[mcftacademics].dbo.Get_UserRoles", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var roles = new List<string>();
                command.Parameters.AddWithValue("@userIdentity", Id);
                try
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        roles.Add(reader["roleName"].ToString());
                    }
                }
                catch (SqlException)
                {
                    // XXX: log exception
                    
                }
                connection.Close();
                return roles;
            }

            public LoginStatus Login()
            {
                LoginStatus loggedIn;
                SqlConnection conn = DbConn.GetConnection();
                //Getting the stored procedure from the datbaase and adding the username paramater
                SqlCommand selectCommand = new SqlCommand("[mcftacademics].dbo.Login_Validation", conn);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@uname", this.Username);
                try
                {
                    conn.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.Read()) //If it finds a username
                    {
                        if (Hashing.ValidatePassword(this.Password, reader["Password"].ToString()))
                            //If the password entered matches the hashed password in the database
                        {
                            // a bit of an ugly place to do it, but get the user's ID
                            int id = GetUserId();
                            if (id != -1)
                            {
                                Id = id;
                                var realName = GetUserRealName();
                                RealName = realName;
                                loggedIn = LoginStatus.Success;
                            }
                            else
                            {
                                loggedIn = LoginStatus.IdFailure;
                            }
                        }
                        else
                        {
                            loggedIn = LoginStatus.InvalidPassword;
                        }
                    }
                    else
                    {
                        loggedIn = LoginStatus.NoUser;
                    }
                }
                catch (Exception ex)
                {
                    // XXX: report exceptions
                    loggedIn = LoginStatus.DatabaseFailure;
                }
                finally
                {
                    //close the connection either way
                    conn.Close();
                }
                return loggedIn;
            }
        }
    }
}