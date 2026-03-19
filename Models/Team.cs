using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class Team {
    [Key]
    public int TeamId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public int? LeaderId { get; set; }
    public Member? Leader { get; set; }

    public int? LevelId { get; set; }
    public Level? Level { get; set; }

    public ICollection<Member>? Members { get; set; }
}