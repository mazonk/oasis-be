using Oasis.DTOs.Activity;

namespace Oasis.Services.Interfaces;

public interface IGeminiService {
    Task<ActivityDto> PromptAsync(AiPromptRequestDto request);
}