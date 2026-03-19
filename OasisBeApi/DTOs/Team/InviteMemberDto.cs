namespace Oasis.DTOs.Team;

public class InviteMemberDto {
    public int TeamId { get; set; }
    public string Email { get; set; } = null!;
    public int InviterId { get; set; }
}