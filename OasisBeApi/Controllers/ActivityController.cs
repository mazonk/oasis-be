using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Oasis.Services;
using Oasis.Services.Interfaces;

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
}