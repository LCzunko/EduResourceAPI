using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduResourceAPI.Auth
{
    public class JwtAuth : IJwtAuth
    {
        public JwtAuth(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        private string Key { get; }

        public async Task<string> GenerateJwtToken(IdentityUser user, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Key);

            var claims = await GetValidClaims(user, userManager, roleManager);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var tokenString = jwtTokenHandler.WriteToken(token);

            return tokenString;
        }

        private async Task<List<Claim>> GetValidClaims(IdentityUser user, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName),
            };

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            claims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }
    }
}
