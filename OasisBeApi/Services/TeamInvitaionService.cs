using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oasis.DTOs.Team;
using Oasis.Models;
using Oasis.Repositories.Interfaces;
using Oasis.Services.Interfaces;

namespace Oasis.Services;

public class TeamInvitationService : ITeamInvitationService {
    private readonly ITeamInvitationRepository _repository;
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<TeamInvitationService> _logger;

    public TeamInvitationService(
        ITeamInvitationRepository repository,
        ITeamRepository teamRepository,
        ILogger<TeamInvitationService> logger) {
        _repository = repository;
        _teamRepository = teamRepository;
        _logger = logger;
    }

    public async Task InviteMemberAsync(InviteMemberDto dto) {
        try {
            // TODO: Add member repository to check if member exists before inviting
            var team = await _teamRepository.GetTeamByIdAsync(dto.TeamId);
            if (team == null) throw new Exception("Team not found");

            if (team.LeaderId != dto.InviterId)
                throw new Exception("Only the team leader can invite members");

            if (await _repository.ExistsAsync(dto.TeamId, dto.Email))
                throw new Exception("This member has already been invited");

            var invitation = new TeamInvitation {
                TeamId = dto.TeamId,
                Email = dto.Email,
                InvitedById = dto.InviterId
            };

            await _repository.AddAsync(invitation);
            _logger.LogInformation("Invitation created for {Email} in team {TeamId}", dto.Email, dto.TeamId);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error while inviting member {Email} to team {TeamId}", dto.Email, dto.TeamId);
            throw;
        }
    }

    public async Task<List<TeamInvitation>> GetInvitationsByTeamAsync(int teamId) {
        try {
            return await _repository.GetByTeamIdAsync(teamId);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error while retrieving invitations for team {TeamId}", teamId);
            throw;
        }
    }

    public async Task<List<TeamInvitationDto>> GetActiveInvitesAsync(string memberEmail)
    {
        if (string.IsNullOrWhiteSpace(memberEmail))
            throw new ArgumentException("Email cannot be empty.", nameof(memberEmail));

        try
        {
            var invites = await _repository.GetActiveInvitationsByEmailAsync(memberEmail);

            var result = invites.Select(i => new TeamInvitationDto {
                InvitationId = i.InvitationId,
                Email = i.Email,
                TeamName = i.Team.Name,
                InvitedByName = $"{i.InvitedBy.Fname} {i.InvitedBy.Lname}",
                CreatedAt = i.CreatedAt
            }).ToList();

            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error while retrieving active invites for member {Email}", memberEmail);
            throw;
        }
    }
}