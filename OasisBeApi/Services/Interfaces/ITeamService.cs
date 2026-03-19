using Oasis.DTOs.Team;

namespace Oasis.Services.Interfaces;

public interface ITeamService {
    Task<TeamDto> CreateTeamAsync(CreateTeamDto dto);
    Task<List<TeamDto>> GetAllTeamsAsync();
    Task<TeamDto?> GetTeamByIdAsync(int id);
}