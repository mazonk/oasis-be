using Microsoft.AspNetCore.Mvc;
using Oasis.Services.Interfaces;

namespace Oasis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LevelController : ControllerBase {
    private readonly ILevelService _levelService;
    private readonly ILogger<LevelController> _logger;

    public LevelController(ILevelService levelService, ILogger<LevelController> logger) {
        _levelService = levelService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        try {
            var levels = await _levelService.GetAllAsync();
            return Ok(levels);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching all levels");
            return StatusCode(500, "Failed to fetch levels");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        try {
            var level = await _levelService.GetByIdAsync(id);
            return Ok(level);
        } catch (KeyNotFoundException) {
            return NotFound($"Level {id} not found");
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching level {LevelId}", id);
            return StatusCode(500, "Failed to fetch level");
        }
    }
}