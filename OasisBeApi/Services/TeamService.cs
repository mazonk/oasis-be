using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Oasis.DTOs.Team;
using Oasis.Models;
using Oasis.Repositories;
using Oasis.Repositories.Interfaces;
using Oasis.Services.Interfaces;

namespace Oasis.Services;

public class TeamService : ITeamService {
    private readonly ITeamRepository _repo;
    private readonly ILogger<TeamService> _logger;

    public TeamService(ITeamRepository repo, ILogger<TeamService> logger) {
        _repo = repo;
        _logger = logger;
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamDto dto) {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        try {
            var team = new Team {
                Name = dto.Name,
                Description = dto.Description,
                LeaderId = dto.LeaderId,
                LevelId = 1 // Default to "Getting Started" level for new teams
            };

            var addedTeam = await _repo.AddTeamAsync(team);

            return new TeamDto {
                TeamId = addedTeam.TeamId,
                Name = addedTeam.Name,
                Description = addedTeam.Description,
                LeaderId = addedTeam.LeaderId,
                LevelId = addedTeam.LevelId
            };
        } catch (DbUpdateException dbEx) {
            _logger.LogError(dbEx, "Database error while creating team {TeamName}", dto.Name);
            throw new InvalidOperationException("Could not save team to the database", dbEx);
        } catch (Exception ex) {
            _logger.LogError(ex, "Unexpected error while creating team {TeamName}", dto.Name);
            throw;
        }
    }

    public async Task<List<TeamDto>> GetAllTeamsAsync() {
        try {
            var teams = await _repo.GetAllTeamsAsync();
            return teams.Select(t => new TeamDto {
                TeamId = t.TeamId,
                Name = t.Name,
                Description = t.Description,
                LeaderId = t.LeaderId,
                LevelId = t.LevelId
            }).ToList();
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching all teams");
            throw;
        }
    }

    public async Task<TeamDto?> GetTeamByIdAsync(int id) {
        try {
            var team = await _repo.GetTeamByIdAsync(id);
            if (team == null) return null;

            return new TeamDto {
                TeamId = team.TeamId,
                Name = team.Name,
                Description = team.Description,
                LeaderId = team.LeaderId,
                LevelId = team.LevelId
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching team by id {TeamId}", id);
            throw;
        }
    }
}