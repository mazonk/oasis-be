namespace Oasis.DTOs.Auth;

public class AuthResponseDto {
    public string Token { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string MemberId { get; set; } = null!;
    public string Message { get; set; } = null!;
}