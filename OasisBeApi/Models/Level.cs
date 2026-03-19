using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class Level {
    [Key]
    public int LevelId { get; set; }
    public string? Name { get; set; }
    public int MinExp { get; set; }

    public ICollection<Team>? Teams { get; set; }
    public ICollection<Member>? Members { get; set; }
}