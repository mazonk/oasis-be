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
    private readonly IMemberRepository _memberRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ILevelRepository _levelRepository;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(IActivityRepository repo, IMemberRepository memberRepository, ITeamRepository teamRepository, ILevelRepository levelRepository, ILogger<ActivityService> logger) {
        _repo = repo;
        _memberRepository = memberRepository;
        _teamRepository = teamRepository;
        _levelRepository = levelRepository;
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

    public async Task<CompleteActivityResultDto> CompleteActivityAsync(int activityId, int memberId) {
        var activity = await _repo.GetActivityByIdAsync(activityId)
            ?? throw new KeyNotFoundException("Activity not found");

        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new KeyNotFoundException("Member not found");

        if (member.TeamId == null)
            throw new InvalidOperationException("Member is not part of a team");

        var xpEarned = activity.Experience;

        // Record member completion
        await _repo.AddMemberActivityAsync(new MemberActivity {
            MemberId = memberId,
            ActivityId = activityId,
            XpEarned = xpEarned,
            CompletedAt = DateTime.UtcNow
        });

        // Record team completion
        await _repo.AddTeamActivityAsync(new TeamActivity {
            TeamId = member.TeamId.Value,
            ActivityId = activityId,
            XpEarned = xpEarned,
            CompletedByMemberId = memberId,
            CompletedAt = DateTime.UtcNow
        });

        // Update member XP and check level up
        member.Experience += xpEarned;
        var previousLevelId = member.LevelId;
        await _levelRepository.GetHighestQualifyingLevelAsync(member.Experience).ContinueWith(t => {
            if (t.Result != null) member.LevelId = t.Result.LevelId;
        });
        await _memberRepository.UpdateAsync(member);

        // Update team XP
        var team = await _teamRepository.GetTeamByIdAsync(member.TeamId.Value)
            ?? throw new KeyNotFoundException("Team not found");
        team.Experience += xpEarned;
        await _teamRepository.UpdateAsync(team);

        var leveledUp = member.LevelId != previousLevelId;
        var newLevel = leveledUp ? await _levelRepository.GetByIdAsync(member.LevelId ?? 0) : null;

        _logger.LogInformation(
            "Member {MemberId} completed activity {ActivityId}, earned {Xp} XP",
            memberId, activityId, xpEarned);

        return new CompleteActivityResultDto {
            XpEarned = xpEarned,
            MemberTotalExperience = member.Experience,
            TeamTotalExperience = team.Experience,
            LeveledUp = leveledUp,
            NewLevelName = newLevel?.Name,
            CompletedAt = DateTime.UtcNow
        };
    }
}