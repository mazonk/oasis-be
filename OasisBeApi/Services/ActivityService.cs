using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Oasis.DTOs.Activity;
using Oasis.Models;
using Oasis.Repositories;
using Oasis.Repositories.Interfaces;
using Oasis.Services.Interfaces;

namespace Oasis.Services;

public class ActivityService : IActivityService {
    private readonly IActivityRepository _repo;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(IActivityRepository repo, ILogger<ActivityService> logger) {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<ActivityDto>> GetAllActivitiesAsync() {
        try {
            var activities = await _repo.GetAllActivitiesAsync();
            return activities.Select(a => new ActivityDto {
                ActivityId = a.ActivityId,
                Title = a.Title,
                Description = a.Description,
                MinMember = a.MinMember ?? 0,
                MaxMember = a.MaxMember ?? 0,
                Experience = a.Experience,
                CategoryName = a.Category.Name
            }).ToList();
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching all activities");
            throw;
        }
    }

    public async Task<ActivityDto?> GetActivityByIdAsync(int id) {
        try {
            var activity = await _repo.GetActivityByIdAsync(id);
            if (activity == null) return null;

            return new ActivityDto {
                ActivityId = activity.ActivityId,
                Title = activity.Title,
                Description = activity.Description,
                MinMember = activity.MinMember ?? 0,
                MaxMember = activity.MaxMember ?? 0,
                Experience = activity.Experience,
                CategoryName = activity.Category.Name
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching activity by id {ActivityId}", id);
            throw;
        }
    }
}