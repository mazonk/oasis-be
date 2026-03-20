namespace Oasis.DTOs.Member;

public class MemberDto {
    public int MemberId { get; set; }
    public string Fname { get; set; } = null!;
    public string Lname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime? Dob { get; set; }
    public string? TeamName { get; set; }
    public int? Experience { get; set; }
    public int? LevelId { get; set; }
    public string? LevelName { get; set; }
}