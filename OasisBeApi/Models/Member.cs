using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class Member {
    [Key]
    public int MemberId { get; set; }

    [Required]
    public string Fname { get; set; } = null!;

    [Required]
    public string Lname { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;
    
    public string Phone { get; set; } = null!;
    public DateTime? Dob { get; set; }

    public int? LevelId { get; set; }
    public Level? Level { get; set; }

    public int? TeamId { get; set; }
    public Team? Team { get; set; }

    public ICollection<MemberMood>? MemberMoods { get; set; }
    public User? User { get; set; }
}