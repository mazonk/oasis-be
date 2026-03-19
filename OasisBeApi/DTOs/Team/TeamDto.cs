namespace Oasis.DTOs.Team;

public class TeamDto {
    public int TeamId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int LeaderId { get; set; }
    public int LevelId { get; set; }
}