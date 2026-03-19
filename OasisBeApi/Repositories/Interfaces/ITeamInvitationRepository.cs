using Oasis.Models;

namespace Oasis.Repositories.Interfaces;

public interface ITeamInvitationRepository {
    Task<TeamInvitation> AddAsync(TeamInvitation invitation);
    Task<bool> ExistsAsync(int teamId, string email);
    Task<TeamInvitation?> GetByIdAsync(int id);
    Task<List<TeamInvitation>> GetByTeamIdAsync(int teamId);
    Task<List<TeamInvitation>> GetActiveInvitationsByEmailAsync(string email);
    Task UpdateAsync(TeamInvitation invitation);
}