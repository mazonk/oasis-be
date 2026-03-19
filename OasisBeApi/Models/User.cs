using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oasis.Models;

public class User {
    [Key]
    public int UserId { get; set; }

    [Required]
    [ForeignKey("Member")]
    public int MemberId { get; set; }

    [Required]
    public string Email { get; set; } = null!; // login email

    [Required]
    public byte[] PasswordHash { get; set; } = null!;

    [Required]
    public byte[] PasswordSalt { get; set; } = null!;

    [Required]
    public string Role { get; set; } = "Member";

    public Member Member { get; set; } = null!;
}