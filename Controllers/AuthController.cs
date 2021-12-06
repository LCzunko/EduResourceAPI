using EduResourceAPI.Auth;
using EduResourceAPI.Models.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduResourceAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtAuth _jwtAuth;

        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IJwtAuth jwtAuth)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtAuth = jwtAuth;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        [Route("register/admin")]
        public async Task<IActionResult> RegisterAdmin(AuthRegistrationDTO admin)
        {
            var existingUser = await _userManager.FindByEmailAsync(admin.Email);
            if (existingUser != null) throw new BadHttpRequestException("Email address already in use.");

            var newAdmin = new IdentityUser() { Email = admin.Email, UserName = admin.UserName };

            var isCreated = await _userManager.CreateAsync(newAdmin, admin.Password);
            if (!isCreated.Succeeded) throw new InvalidOperationException(string.Join('\n', isCreated.Errors));

            var isAdmin = await _userManager.AddToRoleAsync(newAdmin, "Admin");
            if (!isAdmin.Succeeded) throw new InvalidOperationException(string.Join('\n', isAdmin.Errors));

            var isUser = await _userManager.AddToRoleAsync(newAdmin, "User");
            if (!isUser.Succeeded) throw new InvalidOperationException(string.Join('\n', isUser.Errors));

            string jwtToken = await _jwtAuth.GenerateJwtToken(newAdmin, _userManager, _roleManager);

            _logger.LogInformation($"New admin registered - {admin.Email}");
            return Ok(new { BearerToken = jwtToken });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(AuthRegistrationDTO user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null) throw new BadHttpRequestException("Email address already in use.");

            var newUser = new IdentityUser() { Email = user.Email, UserName = user.UserName };

            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (!isCreated.Succeeded) throw new InvalidOperationException(string.Join('\n', isCreated.Errors));

            var isUser = await _userManager.AddToRoleAsync(newUser, "User");
            if (!isUser.Succeeded) throw new InvalidOperationException(string.Join('\n', isUser.Errors));

            string jwtToken = await _jwtAuth.GenerateJwtToken(newUser, _userManager, _roleManager);

            _logger.LogInformation($"New user registered - {user.Email}");
            return Ok(new { BearerToken = jwtToken });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthLoginDTO user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null) return Unauthorized();

            bool isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (!isCorrect) return Unauthorized();

            string jwtToken = await _jwtAuth.GenerateJwtToken(existingUser, _userManager, _roleManager);

            _logger.LogInformation($"User logged in - {user.Email}");
            return Ok(new { BearerToken = jwtToken });
        }
    }
}
