using EduResourceAPI.Auth;
using EduResourceAPI.Controllers.Errors;
using EduResourceAPI.Models.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(AuthRegistrationDTO admin)
        {
            var existingUser = await _userManager.FindByEmailAsync(admin.Email);
            if (existingUser != null)
            {
                Dictionary<string, string> errors = new() { { "Email", "Email address already in use." } };
                var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return BadRequest(new BadRequestError(traceId, errors));
            }

            var newAdmin = new IdentityUser() { Email = admin.Email, UserName = admin.UserName };

            var isCreated = await _userManager.CreateAsync(newAdmin, admin.Password);
            if (!isCreated.Succeeded) throw new InvalidOperationException(string.Join('\n', isCreated.Errors));

            var isAdmin = await _userManager.AddToRoleAsync(newAdmin, "Admin");
            if (!isAdmin.Succeeded) throw new InvalidOperationException(string.Join('\n', isAdmin.Errors));

            var isUser = await _userManager.AddToRoleAsync(newAdmin, "User");
            if (!isUser.Succeeded) throw new InvalidOperationException(string.Join('\n', isUser.Errors));

            string jwtToken = await _jwtAuth.GenerateJwtToken(newAdmin, _userManager, _roleManager);

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - {admin.Email}");
            return Ok(new { BearerToken = jwtToken });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRegistrationDTO user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                Dictionary<string, string> errors = new() { { "Email", "Email address already in use." } };
                var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return BadRequest(new BadRequestError(traceId, errors));
            }

            var newUser = new IdentityUser() { Email = user.Email, UserName = user.UserName };

            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (!isCreated.Succeeded) throw new InvalidOperationException(string.Join('\n', isCreated.Errors));

            var isUser = await _userManager.AddToRoleAsync(newUser, "User");
            if (!isUser.Succeeded) throw new InvalidOperationException(string.Join('\n', isUser.Errors));

            string jwtToken = await _jwtAuth.GenerateJwtToken(newUser, _userManager, _roleManager);

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - {user.Email}");
            return Ok(new { BearerToken = jwtToken });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthLoginDTO user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                Dictionary<string, string> errors = new() { { "Email", "Email address already in use." } };
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return Unauthorized();
            }

            bool isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (!isCorrect)
            {
                Dictionary<string, string> errors = new() { { "Password", "Invalid password for user." } };
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return Unauthorized();
            }

            string jwtToken = await _jwtAuth.GenerateJwtToken(existingUser, _userManager, _roleManager);

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - {user.Email}");
            return Ok(new { BearerToken = jwtToken });
        }
    }
}
