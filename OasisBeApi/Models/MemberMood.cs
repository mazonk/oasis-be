using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class MemberMood {
    [Key, Column(Order = 0)]
    public int MoodId { get; set; }
    public Mood Mood { get; set; } = null!;

    [Key, Column(Order = 1)]
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public string? Cause { get; set; }
}