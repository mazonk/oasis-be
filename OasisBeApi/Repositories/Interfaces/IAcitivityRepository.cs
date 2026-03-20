using Oasis.Models;

namespace Oasis.Repositories.Interfaces;

public interface IActivityRepository {
    public Task<List<Activity>> GetAllActivitiesAsync();
    public Task<Activity?> GetActivityByIdAsync(int id);
    Task AddMemberActivityAsync(MemberActivity memberActivity);
    Task AddTeamActivityAsync(TeamActivity teamActivity);
}