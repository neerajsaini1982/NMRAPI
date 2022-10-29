using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NMRAPIs.BusinessLogic.Helpers
{
    /// <summary>
    /// Extension on<see cref="ClaimsPrincipal"/> class.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Extension Method to get the Login Users Email.
        /// </summary>
        /// <param name="claimsPrincipal">ClaimsPrincipal.</param>
        /// <returns>The Logged in User's Email Address.</returns>
        public static string GetLoggedInUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return claimsPrincipal.FindFirst(ClaimTypes.Email).Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
