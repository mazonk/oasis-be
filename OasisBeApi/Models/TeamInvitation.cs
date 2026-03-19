using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class TeamInvitation {
    [Key]
    public int InvitationId { get; set; }

    [Required]
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public int InvitedById { get; set; } // the leader or member sending the invite
    public Member InvitedBy { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AcceptedAt { get; set; } // null if not accepted yet

    [Required]
    public bool IsAccepted { get; set; } = false;
}