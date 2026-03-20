using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Oasis.Services;
using Oasis.Services.Interfaces;
using Oasis.DTOs.Activity;

namespace Oasis.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ActivityController : ControllerBase {
    private readonly IActivityService _service;
    private readonly ILogger<ActivityController> _logger;

    public ActivityController(IActivityService service, ILogger<ActivityController> logger) {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActivities() {
        try {
            var activities = await _service.GetAllActivitiesAsync();
            return Ok(activities);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching all activities");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityById(int id) {
        try {
            var activity = await _service.GetActivityByIdAsync(id);
            if (activity == null) return NotFound();
            return Ok(activity);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching activity by id {ActivityId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteActivity(int id, [FromBody] CompleteActivityRequestDto request) {
        try {
            var result = await _service.CompleteActivityAsync(id, request.MemberId);
            return Ok(result);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error completing activity {ActivityId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}