namespace Oasis.DTOs.Auth;

public class RegisterUserDto {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FName { get; set; } = null!;
    public string LName { get; set; } = null!;
}