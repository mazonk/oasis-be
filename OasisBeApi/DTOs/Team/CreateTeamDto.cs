using System.ComponentModel.DataAnnotations;

namespace Oasis.DTOs.Team;

public class CreateTeamDto {
    [Required]
    [MinLength(3)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public int LeaderId { get; set; } // The creator becomes the team leader
}