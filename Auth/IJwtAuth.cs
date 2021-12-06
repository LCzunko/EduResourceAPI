using Microsoft.AspNetCore.Identity;

namespace EduResourceAPI.Auth
{
    public interface IJwtAuth
    {
        Task<string> GenerateJwtToken(IdentityUser user, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager);
    }
}