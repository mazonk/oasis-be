using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oasis.DTOs.Member;
using Oasis.Services.Interfaces;
using System.Security.Claims;

namespace Oasis.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MemberController : ControllerBase {
    private readonly IMemberService _service;

    public MemberController(IMemberService service) {
        _service = service;
    }

    // Get current user
    [HttpGet]
    public async Task<IActionResult> GetMe() {
        try {
            var memberId = GetMemberIdFromToken();
            var member = await _service.GetByIdAsync(memberId);
            return Ok(member);
        }
        catch (KeyNotFoundException ex) {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex) {
            return StatusCode(500, new { Message = "Error retrieving member", Details = ex.Message });
        }
    }

    // Get member by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        try {
            var member = await _service.GetByIdAsync(id);
            return Ok(member);
        }
        catch (KeyNotFoundException ex) {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex) {
            return StatusCode(500, new { Message = "Error retrieving member", Details = ex.Message });
        }
    }

    // Update current user
    [HttpPut]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateMemberDto dto) {
        try {
            var memberId = GetMemberIdFromToken();
            await _service.UpdateAsync(memberId, dto);
            return Ok(new { Message = "Member updated successfully" });
        }
        catch (KeyNotFoundException ex) {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex) {
            return StatusCode(500, new { Message = "Error updating member", Details = ex.Message });
        }
    }

    private int GetMemberIdFromToken() {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new UnauthorizedAccessException("Invalid token");

        return int.Parse(claim.Value);
    }
}