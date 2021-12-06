using Microsoft.AspNetCore.Identity;

namespace EduResourceAPI.Auth
{
    public interface IJwtAuth
    {
        string GenerateJwtToken(IdentityUser user);
    }
}