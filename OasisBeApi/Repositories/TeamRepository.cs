using Microsoft.EntityFrameworkCore;

using Oasis.Repositories.Interfaces;
using Oasis.Data;
using Oasis.Models;

namespace Oasis.Repositories;

public class TeamRepository : ITeamRepository {
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Team> AddTeamAsync(Team team) {
        if (await _context.Teams.AnyAsync(t => t.Name == team.Name))
        {
            throw new InvalidOperationException("A team with this name already exists.");
        }

        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();
        return team;
    }

    public async Task<List<Team>> GetAllTeamsAsync() {
        return await _context.Teams
            .Include(t => t.Leader)
            .Include(t => t.Level)
            .ToListAsync();
    }

    public async Task<Team?> GetTeamByIdAsync(int id) {
        return await _context.Teams
            .Include(t => t.Leader)
            .Include(t => t.Level)
            .FirstOrDefaultAsync(t => t.TeamId == id);
    }
    
    public async Task UpdateAsync(Team team) {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }
}