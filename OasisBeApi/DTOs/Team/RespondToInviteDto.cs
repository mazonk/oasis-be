using System.ComponentModel.DataAnnotations;

namespace Oasis.DTOs.Team;

public class RespondToInviteDto {
    [Required]
    public int InvitationId { get; set; }

    [Required]
    public bool Accept { get; set; } // true = accept, false = decline
}