using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class ActivityCategory {
    [Key]
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Activity>? Activities { get; set; }
}