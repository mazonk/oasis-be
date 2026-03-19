using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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

    [HttpPost("respond")]
    public async Task<IActionResult> Respond([FromBody] RespondToInviteDto dto) {
        var memberIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (memberIdClaim == null)
            return Unauthorized("JWT does not contain member ID.");

        var memberId = int.Parse(memberIdClaim.Value);

        try {
            var accepted = await _service.RespondToInviteAsync(memberId, dto);
            return Ok(new { Accepted = accepted });
        }
        catch (KeyNotFoundException ex) {
            return NotFound(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex) {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex) {
            // fallback for unexpected errors
            return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
        }
    }

    [HttpPost("leave")]
    public async Task<IActionResult> LeaveTeam() {
        var memberIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (memberIdClaim == null)
            return Unauthorized("Invalid token");

        var memberId = int.Parse(memberIdClaim.Value);

        try {
            await _service.LeaveTeamAsync(memberId);
            return Ok(new { Message = "Successfully left the team" });
        }
        catch (KeyNotFoundException ex) {
            return NotFound(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex) {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex) {
            return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
        }
    }
}