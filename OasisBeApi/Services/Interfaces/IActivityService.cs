using Oasis.DTOs.Activity;

namespace Oasis.Services.Interfaces;

public interface IActivityService {
    Task<List<ActivityDto>> GetAllActivitiesAsync();
    Task<ActivityDto> GetActivityByIdAsync(int id);
    Task<CompleteActivityResultDto> CompleteActivityAsync(int activityId, int memberId);
}