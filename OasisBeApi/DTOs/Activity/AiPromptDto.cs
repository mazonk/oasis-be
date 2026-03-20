namespace Oasis.DTOs.Activity;

public class AiPromptRequestDto {
    public string? Location { get; set; }
    public bool IncludeWeather { get; set; } = false;
    public int? TeamSize { get; set; }
}

public class AiPromptResponseDto {
    public string Response { get; set; } = string.Empty;
    public string ContextUsed { get; set; } = string.Empty;
}