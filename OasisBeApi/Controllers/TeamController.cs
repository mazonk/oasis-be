using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Oasis.DTOs.Team;
using Oasis.Services;
using Oasis.Services.Interfaces;

namespace Oasis.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TeamController : ControllerBase {
    private readonly ITeamService _service;
    private readonly ILogger<TeamController> _logger;

    public TeamController(ITeamService service, ILogger<TeamController> logger) {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDto dto) {
        if (!ModelState.IsValid) {
            _logger.LogWarning("Invalid team creation attempt: {@Dto}", dto);
            return BadRequest(ModelState);
        }

        try {
            var team = await _service.CreateTeamAsync(dto);
            return CreatedAtAction(nameof(GetTeamById), new { id = team.TeamId }, team);
        } catch (InvalidOperationException ioEx) {
            _logger.LogError(ioEx, "Could not create team due to database error");
            return StatusCode(500, ioEx.Message);
        } catch (Exception ex) {
            _logger.LogError(ex, "Unexpected error creating team");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeams() {
        try {
            var teams = await _service.GetAllTeamsAsync();
            return Ok(teams);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching all teams");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeamById(int id) {
        try {
            var team = await _service.GetTeamByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching team by id {TeamId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetTeamByMemberId(int memberId) {
        try {
            var team = await _service.GetTeamByMemberIdAsync(memberId);
            if (team == null) return NotFound();
            return Ok(team);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching team by member id {MemberId}", memberId);
            return StatusCode(500, "Internal server error");
        }
    }
}