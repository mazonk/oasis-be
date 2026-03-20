using Oasis.Models;

namespace Oasis.Repositories.Interfaces;

public interface ITeamRepository {
    Task<Team> AddTeamAsync(Team team);
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(int id);
    Task UpdateAsync(Team team);
}