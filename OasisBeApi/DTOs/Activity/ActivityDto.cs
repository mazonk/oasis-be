namespace Oasis.DTOs.Activity;

public class ActivityDto {
    public int ActivityId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int MinMember { get; set; }
    public int MaxMember { get; set; }
    public int Experience { get; set; }
    public string CategoryName { get; set; } = null!;
}
