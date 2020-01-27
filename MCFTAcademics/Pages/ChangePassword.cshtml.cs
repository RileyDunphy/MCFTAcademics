﻿using System;
using System.Collections.Generic;
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
        public string OldPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string NewPasswordRepeat { get; set; }

        public IActionResult OnPost()
        {
            int userId;
            if (!int.TryParse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value, out userId))
            {
                ModelState.AddModelError("", "The user ID couldn't be parsed.");
                // this one is really fatal
                return Page();
            }
            bool error = false;
            try
            {
                if (!PasswordMatches(userId, OldPassword))
                {
                    ModelState.AddModelError("", "The current password isn't valid.");
                    error = true;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("",
                    "There was an exception from the system checking the password;" +
                    "report this to an administrator: " + e.Message);
                error = true;
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One of the values is blank.");
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

            var newPasswordHashed = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            try
            {
                if (!ChangePassword(userId, newPasswordHashed))
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

        // XXX: There is duplication from this and Login page, should be deduped in Login class
        bool PasswordMatches(int userId, string currentPassword)
        {
            SqlConnection connection = null;
            try
            {
                connection = DAL.DbConn.GetConnection();
                connection.Open();
                // XXX: UDF
                var sql = "select password from mcftacademics.dbo.users where userId = @userId";
                var query = connection.CreateCommand();
                query.CommandText = sql;
                query.Parameters.AddWithValue("@userId", userId);
                var dbPassword = query.ExecuteScalar().ToString();
                return BCrypt.Net.BCrypt.Verify(currentPassword, dbPassword);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        bool ChangePassword(int userId, string newPassword)
        {
            SqlConnection connection = null;
            try
            {
                connection = DAL.DbConn.GetConnection();
                connection.Open();
                // XXX: UDF
                var sql = "update mcftacademics.dbo.users set password = @newPassword where userId = @userId";
                var query = connection.CreateCommand();
                query.CommandText = sql;
                query.Parameters.AddWithValue("@userId", userId);
                query.Parameters.AddWithValue("@newPassword", newPassword);
                return query.ExecuteNonQuery() > 0;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
    }
}