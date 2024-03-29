using static  CarRentingSystem.Areas.Admin.Constants.AdminConstants;
using System.Security.Claims;

namespace CarRentingSystem.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAdmin(this ClaimsPrincipal user) 
            => user.IsInRole(AdminRoleName);

    }
}
