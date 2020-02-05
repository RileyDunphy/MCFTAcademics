using System;
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
        public IActionResult OnPostGrant(int userId, string roleName)
        {
            var user = BL.User.GetUser(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "There is no user with that ID.");
                return Page();
            }
            // see comment on Grant for if this should really be some kind of static func
            // instead of ctor then grant on instance
            var role = new BL.Role(roleName, -1, user);
            if (role == null)
            {
                ModelState.AddModelError("", "There is no role with that ID.");
                return Page();
            }
            // we don't need to keep the new object around, we just care if it's null
            if (role.Grant() == null)
            {
                ModelState.AddModelError("", "The role couldn't be granted.");
                return Page();
            }
            SetViewData(user);
            return Page();
        }

        // GET because it's from a link, not a form
        public IActionResult OnGetRevoke(int userId, int roleId)
        {
            var user = BL.User.GetUser(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "There is no user with that ID.");
                return Page();
            }
            var role = user.GetRole(roleId);
            if (role == null)
            {
                ModelState.AddModelError("", "There is no role with that ID.");
                return Page();
            }
            if (!role.Revoke())
            {
                ModelState.AddModelError("", "The role couldn't be revoked.");
                return Page();
            }
            SetViewData(user);
            return Page();
        }

        public void SetViewData(BL.User user)
        {
            ViewData["ViewRoles_TargetUser"] = user;
            // the param here is the current session user principal, being invoked on the queried user object
            ViewData["ViewRoles_TargetDescription"] = user.GetReferentialName(User);
        }

        public void OnGetWithId(int id)
        {
            var user = BL.User.GetUser(id);
            SetViewData(user);
        }
    }
}