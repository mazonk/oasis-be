using Microsoft.EntityFrameworkCore;
using Oasis.Repositories.Interfaces;
using Oasis.Data;
using Oasis.Models;

namespace Oasis.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly ILogger<ActivityRepository> _logger;
    private readonly AppDbContext _context;

    public ActivityRepository(AppDbContext context, ILogger<ActivityRepository> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Activity>> GetAllActivitiesAsync() {
        return await _context.Activities.Include(a => a.Category).ToListAsync();
    }

    public async Task<Activity?> GetActivityByIdAsync(int id) {
        return await _context.Activities.Include(a => a.Category).FirstOrDefaultAsync(a => a.ActivityId == id);
    }

    public async Task AddMemberActivityAsync(MemberActivity memberActivity) {
        await _context.MemberActivities.AddAsync(memberActivity);
        await _context.SaveChangesAsync();
    }

    public async Task AddTeamActivityAsync(TeamActivity teamActivity) {
        await _context.TeamActivities.AddAsync(teamActivity);
        await _context.SaveChangesAsync();
    }
}