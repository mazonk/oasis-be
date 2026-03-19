using Microsoft.EntityFrameworkCore;
using Oasis.Data;
using Oasis.Models;
using Oasis.Repositories.Interfaces;

namespace Oasis.Repositories;

public class TeamInvitationRepository : ITeamInvitationRepository {
    private readonly AppDbContext _context;

    public TeamInvitationRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<TeamInvitation> AddAsync(TeamInvitation invitation) {
        _context.TeamInvitations.Add(invitation);
        await _context.SaveChangesAsync();
        return invitation;
    }

    public async Task<bool> ExistsAsync(int teamId, string email) {
        return await _context.TeamInvitations
            .AnyAsync(x => x.TeamId == teamId && x.Email.ToLower() == email.ToLower() && !x.IsAccepted);
    }

    public async Task<List<TeamInvitation>> GetByTeamIdAsync(int teamId) {
        return await _context.TeamInvitations
            .Where(x => x.TeamId == teamId)
            .ToListAsync();
    }

    public async Task<List<TeamInvitation>> GetActiveInvitationsByEmailAsync(string email) {
        return await _context.TeamInvitations
            .Include(ti => ti.Team)
            .Include(ti => ti.InvitedBy)
            .Where(ti => ti.Email == email && !ti.IsAccepted)
            .ToListAsync();
    }
}