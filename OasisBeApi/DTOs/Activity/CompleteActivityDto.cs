namespace Oasis.DTOs.Activity;

public class CompleteActivityRequestDto {
    public int MemberId { get; set; }
}

public class CompleteActivityResultDto {
    public int XpEarned { get; set; }
    public int MemberTotalExperience { get; set; }
    public string? NewLevelName { get; set; }
    public bool LeveledUp { get; set; }
    public int TeamTotalExperience { get; set; }
    public DateTime CompletedAt { get; set; }
}