using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class MemberActivity {
    [Key]
    public int MemberActivityId { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public int XpEarned { get; set; }
}