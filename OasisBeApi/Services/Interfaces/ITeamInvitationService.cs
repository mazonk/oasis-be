using Oasis.DTOs.Team;
using Oasis.Models;

namespace Oasis.Services.Interfaces;

public interface ITeamInvitationService {
    Task InviteMemberAsync(InviteMemberDto dto);
    Task<List<TeamInvitation>> GetInvitationsByTeamAsync(int teamId);
    Task<List<TeamInvitationDto>> GetActiveInvitesAsync(string memberEmail);
    Task<bool> RespondToInviteAsync(int memberId, RespondToInviteDto dto);
    Task LeaveTeamAsync(int memberId);
}