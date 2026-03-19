using Microsoft.AspNetCore.Mvc;
using Oasis.Services.Interfaces;
using Oasis.DTOs.Auth;
using Oasis.Services;

namespace Oasis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IAuthService _auth;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService auth, ILogger<AuthController> logger) {
        _auth = auth;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto) {
        try {
            var response = await _auth.RegisterAsync(dto);
            if (string.IsNullOrEmpty(response.Token))
                return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Unexpected error during registration for {Email}", dto.Email);
            return StatusCode(500, new AuthResponseDto
            {
                Token = string.Empty,
                Role = string.Empty,
                MemberId = string.Empty,
                Message = "An unexpected error occurred"
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto) {
        try {
            var response = await _auth.LoginAsync(dto);
            if (string.IsNullOrEmpty(response.Token))
                return BadRequest(response);
            return Ok(response);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Unexpected error during login for {Email}", dto.Email);
            return StatusCode(500, new AuthResponseDto
            {
                Token = string.Empty,
                Role = string.Empty,
                MemberId = string.Empty,
                Message = "An unexpected error occurred"
            });
        }
    }
}