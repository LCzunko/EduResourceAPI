using EduResourceAPI.Auth;
using EduResourceAPI.Models.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Http;
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
        private readonly IJwtAuth _jwtAuth;

        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, IJwtAuth jwtAuth)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtAuth = jwtAuth;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(AuthRegistrationDTO user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null) throw new BadHttpRequestException("Email address already in use.");

            var newUser = new IdentityUser() { Email = user.Email, UserName = user.UserName };

            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (!isCreated.Succeeded) throw new InvalidOperationException(string.Join('\n', isCreated.Errors));
                
            string jwtToken = _jwtAuth.GenerateJwtToken(newUser);

            _logger.LogInformation($"New user registered - {user.Email}");
            return Ok(new { BearerToken = jwtToken });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthLoginDTO user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null) return Unauthorized();

            bool isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (!isCorrect) return Unauthorized();

            string jwtToken = _jwtAuth.GenerateJwtToken(existingUser);

            _logger.LogInformation($"User logged in {user.Email}");
            return Ok(new { BearerToken = jwtToken });
        }
    }
}
