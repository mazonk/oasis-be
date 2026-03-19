using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class Team {
    [Key]
    public int TeamId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public int LeaderId { get; set; }

    [Required]
    public Member Leader { get; set; } = null!;

    [Required]
    public int LevelId { get; set; }

    [Required]
    public Level Level { get; set; } = null!;

    public ICollection<Member>? Members { get; set; }
    public ICollection<TeamInvitation>? TeamInvitations { get; set; }
}