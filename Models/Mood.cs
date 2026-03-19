using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class Mood {
    [Key]
    public int MoodId { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<MemberMood>? MemberMoods { get; set; }
}