using Microsoft.AspNetCore.Mvc;
using Oasis.DTOs.Activity;
using Oasis.Services.Interfaces;

namespace Oasis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase {
    private readonly IGeminiService _geminiService;
    private readonly ILogger<AiController> _logger;

    public AiController(IGeminiService geminiService, ILogger<AiController> logger) {
        _geminiService = geminiService;
        _logger = logger;
    }

    [HttpPost("prompt")]
    public async Task<IActionResult> Prompt([FromBody] AiPromptRequestDto request) {
        try {
            var result = await _geminiService.PromptAsync(request);
            return Ok(result);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error processing AI prompt");
            return StatusCode(500, "Failed to process prompt.");
        }
    }
}