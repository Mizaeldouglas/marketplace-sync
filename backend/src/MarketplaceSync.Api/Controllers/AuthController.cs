using Microsoft.AspNetCore.Mvc;
using MarketplaceSync.Service.Auth;
using MarketplaceSync.Api.DTOs.Auth;

namespace MarketplaceSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, token, message) = await _authService.RegisterAsync(
                request.Email, request.Password, request.LojaName);

            if (!success)
                return BadRequest(new { message });

            var response = new AuthResponseDto
            {
                Success = true,
                Message = message,
                Token = token,
                User = new UserDto { Email = request.Email, LojaName = request.LojaName }
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, token, message) = await _authService.LoginAsync(request.Email, request.Password);

            if (!success)
                return Unauthorized(new { message });

            var response = new AuthResponseDto
            {
                Success = true,
                Message = message,
                Token = token,
                User = new UserDto { Email = request.Email }
            };

            return Ok(response);
        }
    }
}
