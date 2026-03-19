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
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<TeamInvitationService> _logger;

    public TeamInvitationService(
        ITeamInvitationRepository repository,
        ITeamRepository teamRepository,
        IMemberRepository memberRepository,
        ILogger<TeamInvitationService> logger) {
        _repository = repository;
        _teamRepository = teamRepository;
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task InviteMemberAsync(InviteMemberDto dto) {
        try {
            var team = await _teamRepository.GetTeamByIdAsync(dto.TeamId);
            if (team == null) throw new Exception("Team not found");

            if (team.LeaderId != dto.InviterId)
                throw new Exception("Only the team leader can invite members");

            var existingMember = await _memberRepository.GetByEmailAsync(dto.Email);
            if (existingMember != null && existingMember.TeamId.HasValue)
                throw new InvalidOperationException("This member is already part of a team");

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

    public async Task<List<TeamInvitationDto>> GetActiveInvitesAsync(string memberEmail) {
        if (string.IsNullOrWhiteSpace(memberEmail))
            throw new ArgumentException("Email cannot be empty.", nameof(memberEmail));

        try {
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

    public async Task<bool> RespondToInviteAsync(int memberId, RespondToInviteDto dto) {
        var invitation = await _repository.GetByIdAsync(dto.InvitationId);
        if (invitation == null) {
            _logger.LogWarning("Invitation {InviteId} not found", dto.InvitationId);
            throw new KeyNotFoundException("Invitation not found");
        }

        if (invitation.IsAccepted || invitation.AcceptedAt.HasValue) {
            _logger.LogWarning("Invitation {InviteId} already responded", dto.InvitationId);
            throw new InvalidOperationException("Invitation already responded to");
        }

        if (dto.Accept) {
            // Accepting the invite
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null) {
                _logger.LogError("Member {MemberId} not found while accepting invite", memberId);
                throw new KeyNotFoundException("Member not found");
            }

            member.TeamId = invitation.TeamId;
            invitation.IsAccepted = true;
            invitation.AcceptedAt = DateTime.UtcNow;

            _logger.LogInformation(
                "Member {MemberId} accepted invite {InviteId} to Team {TeamId}",
                memberId, dto.InvitationId, invitation.TeamId);
        }
        else {
            // Declining the invite
            invitation.IsAccepted = false;
            invitation.AcceptedAt = DateTime.UtcNow;

            _logger.LogInformation(
                "Member {MemberId} declined invite {InviteId} to Team {TeamId}",
                memberId, dto.InvitationId, invitation.TeamId);
        }

        await _repository.UpdateAsync(invitation);
        return invitation.IsAccepted;
    }

    public async Task LeaveTeamAsync(int memberId) {
        try {
            var member = await _memberRepository.GetByIdAsync(memberId);

            if (member == null) {
                _logger.LogWarning("Member {MemberId} not found", memberId);
                throw new KeyNotFoundException("Member not found");
            }

            if (!member.TeamId.HasValue) {
                _logger.LogWarning("Member {MemberId} is not part of a team", memberId);
                throw new InvalidOperationException("You are not part of a team");
            }

            var team = await _teamRepository.GetTeamByIdAsync(member.TeamId.Value);

            if (team == null) {
                _logger.LogError("Team {TeamId} not found for member {MemberId}", member.TeamId, memberId);
                throw new KeyNotFoundException("Team not found");
            }

            // Leader cannot leave
            if (team.LeaderId == memberId) {
                _logger.LogWarning("Leader {MemberId} attempted to leave team {TeamId}", memberId, team.TeamId);
                throw new InvalidOperationException("Team leader cannot leave the team. Transfer leadership or delete the team.");
            }

            member.TeamId = null;

            await _memberRepository.UpdateAsync(member);

            _logger.LogInformation("Member {MemberId} left team {TeamId}", memberId, team.TeamId);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error while member {MemberId} leaving team", memberId);
            throw;
        }
    }
}