using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Oasis.Services.Interfaces;
using Oasis.Repositories.Interfaces;
using Oasis.DTOs.Auth;
using Oasis.Models;
using Oasis.Repositories;

namespace Oasis.Services;

public class AuthService : IAuthService {
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository repo, IConfiguration config, ILogger<AuthService> logger) {
        _repo = repo;
        _config = config;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto) {
        try {
            var existingUser = await _repo.GetUserByEmailAsync(dto.Email);
            if (existingUser != null) {
                _logger.LogWarning("Registration attempt with existing email: {Email}", dto.Email);
                return new AuthResponseDto {
                    Token = string.Empty,
                    Role = string.Empty,
                    MemberId = string.Empty,
                    Message = "Email already registered"
                };
            }

            var member = new Member {
                Fname = dto.FName,
                Lname = dto.LName,
                Email = dto.Email,
                Experience = 0,
                LevelId = 1 // Default to level 1 for new users
            };
            await _repo.AddMemberAsync(member);

            CreatePasswordHash(dto.Password, out var hash, out var salt);

            var user = new User {
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                MemberId = member.MemberId,
                Role = "User"
            };
            await _repo.AddUserAsync(user);

            var token = CreateToken(user);

            _logger.LogInformation("User registered successfully: {Email}", dto.Email);

            return new AuthResponseDto {
                Token = token,
                Role = user.Role,
                MemberId = user.MemberId.ToString(),
                Message = "Registration successful"
            };
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error during registration for {Email}", dto.Email);
            return new AuthResponseDto
            {
                Token = string.Empty,
                Role = string.Empty,
                MemberId = string.Empty,
                Message = "An error occurred during registration"
            };
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto) {
        try {
            var user = await _repo.GetUserByEmailAsync(dto.Email);
            if (user == null || !VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt)) {
                _logger.LogWarning("Failed login attempt for {Email}", dto.Email);
                return new AuthResponseDto
                {
                    Token = string.Empty,
                    Role = string.Empty,
                    MemberId = string.Empty,
                    Message = "Invalid credentials"
                };
            }

            var token = CreateToken(user);

            _logger.LogInformation("User logged in successfully: {Email}", dto.Email);

            return new AuthResponseDto {
                Token = token,
                Role = user.Role,
                MemberId = user.MemberId.ToString(),
                Message = "Login successful"
            };
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error during login for {Email}", dto.Email);
            return new AuthResponseDto {
                Token = string.Empty,
                Role = string.Empty,
                MemberId = string.Empty,
                Message = "An error occurred during login"
            };
        }
    }

    // ---------------------
    // Private helpers
    // ---------------------
    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt) {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt) {
        using var hmac = new HMACSHA512(salt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hash);
    }

    private string CreateToken(User user) {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.MemberId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_SECRET"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}