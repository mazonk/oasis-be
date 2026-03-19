using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Oasis.Services.Interfaces;
using Oasis.DTOs.Team;

namespace Oasis.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TeamInvitationController : ControllerBase {
    private readonly ITeamInvitationService _service;

    public TeamInvitationController(ITeamInvitationService service) {
        _service = service;
    }

    [HttpPost("invite")]
    public async Task<IActionResult> Invite([FromBody] InviteMemberDto dto) {
        try {
            await _service.InviteMemberAsync(dto);
            return Ok(new { Message = "Invitation sent successfully" });
        }
        catch (Exception ex) {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("team/{teamId}")]
    public async Task<IActionResult> GetInvitations(int teamId) {
        var invitations = await _service.GetInvitationsByTeamAsync(teamId);
        return Ok(invitations);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveInvites([FromQuery] string email) {
        try
        {
            var invites = await _service.GetActiveInvitesAsync(email);
            return Ok(invites);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    
}