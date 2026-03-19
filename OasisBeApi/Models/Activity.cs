using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class Activity {
    [Key]
    public int ActivityId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? MinMember { get; set; }
    public int? MaxMember { get; set; }
    public int Experience { get; set; }

    public int? CategoryId { get; set; }
    public ActivityCategory? Category { get; set; }
}