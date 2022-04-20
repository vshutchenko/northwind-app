using System.Security.Claims;

namespace NorthwindMvcApp.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(ClaimTypes.Role, "admin");
        }

        public static bool IsCustomer(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(ClaimTypes.Role, "customer");
        }

        public static bool IsEmployee(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(ClaimTypes.Role, "employee");
        }

        public static bool IsAuthor(this ClaimsPrincipal principal, string authorId)
        {
            var claimValue = principal.FindFirst(c => c.Type == "NorthwindId").Value;
            return claimValue == authorId;
        }
    }
}
