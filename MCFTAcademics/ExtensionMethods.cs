using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MCFTAcademics
{
    /// <summary>
    /// Utility functions for things like user management pages.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets if a provided user ID matches the claims principal
        /// representing the claims principal.
        /// </summary>
        /// <param name="claimsPrincipal">
        /// The claims principal.
        /// </param>
        /// <param name="otherId">
        /// The ID to match the claims principal against, usually sourced from
        /// a User object.
        /// </param>
        /// <returns>
        /// If the principal's ID claim matches the ID given.
        /// </returns>
        /// <remarks>
        /// Usually used for checking if a user ID from a user object is
        /// actually the same one from the session's claims principal
        /// representing the current user.
        /// </remarks>
        public static bool UserIdMatches(this ClaimsPrincipal claimsPrincipal, int otherId)
        {
            if (claimsPrincipal == null)
                throw new ArgumentNullException(nameof(claimsPrincipal));
            var nameIdentifier = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(nameIdentifier))
                return false;
            if (int.TryParse(nameIdentifier, out int claimId))
                return claimId == otherId;
            return false;
        }

        /// <summary>
        /// Says if the user object represents the current user, returning the user's real name if not.
        /// </summary>
        /// <param name="user">The user with a name and ID.</param>
        /// <param name="self">
        /// The claims principal representing the current user, usually sourced from a sessiom.
        /// </param>
        /// <returns>Yourself, nobody, or the user's real name.</returns>
        public static string GetReferentialName(this BL.User user, ClaimsPrincipal self)
        {
            // i don't want to put very claims/web focused code in the User class
            if (user == null)
                return "nobody";
            return UserIdMatches(self, user.Id) ? "yourself" : user.Name;
        }
    }
}
