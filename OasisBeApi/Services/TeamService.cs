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
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<TeamService> _logger;

    public TeamService(ITeamRepository repo, IMemberRepository memberRepository, ILogger<TeamService> logger) {
        _repo = repo;
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamDto dto) {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        try {
            var leader = await _memberRepository.GetByIdAsync(dto.LeaderId);
            if (leader == null) throw new KeyNotFoundException("Leader not found");

            if (leader.TeamId.HasValue) throw new InvalidOperationException("Leader is already part of a team");
            
            var team = new Team {
                Name = dto.Name,
                Description = dto.Description,
                LeaderId = dto.LeaderId,
                Experience = 0,
                LevelId = 1 // Default to "Getting Started" level for new teams
            };

            var addedTeam = await _repo.AddTeamAsync(team);

            leader.TeamId = addedTeam.TeamId;
            await _memberRepository.UpdateAsync(leader);

            return new TeamDto {
                TeamId = addedTeam.TeamId,
                Name = addedTeam.Name,
                Description = addedTeam.Description,
                LeaderId = addedTeam.LeaderId,
                Experience = addedTeam.Experience,
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
                Experience = t.Experience,
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
                Experience = team.Experience,
                LevelId = team.LevelId
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching team by id {TeamId}", id);
            throw;
        }
    }

    public async Task<TeamDto?> GetTeamByMemberIdAsync(int memberId) {
        try {
            var team = await _repo.GetTeamByMemberIdAsync(memberId);
            if (team == null) return null;

            return new TeamDto {
                TeamId = team.TeamId,
                Name = team.Name,
                Description = team.Description,
                LeaderId = team.LeaderId,
                Experience = team.Experience,
                LevelName = team.Level.Name,
                LevelId = team.LevelId
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching team by member id {MemberId}", memberId);
            throw;
        }
    }
}