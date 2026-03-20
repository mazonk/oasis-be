using System.ComponentModel.DataAnnotations;

namespace Oasis.Models;

public class TeamActivity {
    [Key]
    public int TeamActivityId { get; set; }

    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public int XpEarned { get; set; }
    public int CompletedByMemberId { get; set; }
}