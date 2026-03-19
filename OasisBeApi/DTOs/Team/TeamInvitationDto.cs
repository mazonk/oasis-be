namespace Oasis.DTOs.Team;

public class TeamInvitationDto {
    public int InvitationId { get; set; }
    public string Email { get; set; } = null!;
    public string TeamName { get; set; } = null!;
    public string InvitedByName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}