using Newtonsoft.Json;
using System.Security.Claims;

namespace MaratukAdmin.Utils
{
    public static class JWTUserExtractor
    {
        public static  IdentityUserInfo GetUserInfo(ClaimsPrincipal principal)
        {
            if (principal == null)
                return null;

            var user = principal.Claims.FirstOrDefault(c => c.Type == "sid");
            if (user != null && !string.IsNullOrWhiteSpace(user.Value))
            {
                var userInfo = JsonConvert.DeserializeObject<IdentityUserInfo>(user.Value);
                return userInfo;
            }

            return null;

        }

        public static IdentityUserInfo GetAdminUserInfo(ClaimsPrincipal principal)
        {
            if (principal == null)
                return null;

            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && !string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                var userInfo = new IdentityUserInfo
                {
                    UserId = int.Parse(userIdClaim.Value)
                };
                return userInfo;
            }

            return null;

        }
    }

    public class IdentityUserInfo
    {
        public int UserId { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
