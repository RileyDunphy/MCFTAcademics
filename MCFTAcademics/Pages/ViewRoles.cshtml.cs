﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCFTAcademics
{
    [Authorize(Roles = "Admin")]
    public class ViewRolesModel : PageModel
    {
        // XXX: Should really be moved into a controller
        public IActionResult OnPostGrant(int userId, string role)
        {
            throw new NotImplementedException();
        }

        // GET because it's from a link, not a form
        public IActionResult OnGetRevoke(int userId, int roleId)
        {
            throw new NotImplementedException();
        }

        // XXX: This is shared with ChangePassword; it should be broken into a seperate utility library
        public bool TargetUserIsSelf(int id) => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) == id;

        public void OnGetWithId(int id)
        {
            var user = BL.User.GetUser(id);
            ViewData["ViewRoles_TargetUser"] = user;
            ViewData["ViewRoles_TargetDescription"] = TargetUserIsSelf(id) ? "yourself" : (user?.Name ?? "nobody");
        }
    }
}