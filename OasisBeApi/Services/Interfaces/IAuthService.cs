using Oasis.DTOs.Auth;

namespace Oasis.Services.Interfaces;

public interface IAuthService {
    Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}