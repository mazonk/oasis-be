using Microsoft.Extensions.Logging;
using Oasis.DTOs.Member;
using Oasis.Repositories.Interfaces;
using Oasis.Services.Interfaces;

namespace Oasis.Services;

public class MemberService : IMemberService {
    private readonly IMemberRepository _repository;
    private readonly ILogger<MemberService> _logger;

    public MemberService(IMemberRepository repository, ILogger<MemberService> logger) {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MemberDto> GetByIdAsync(int memberId) {
        var member = await _repository.GetByIdAsync(memberId);
        if (member == null) {
            _logger.LogWarning("Member {MemberId} not found", memberId);
            throw new KeyNotFoundException("Member not found");
        }

        return MapToDto(member);
    }

    public async Task<MemberDto> GetByEmailAsync(string email) {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var member = await _repository.GetByEmailAsync(email);
        if (member == null) {
            _logger.LogWarning("Member with email {Email} not found", email);
            throw new KeyNotFoundException("Member not found");
        }

        return MapToDto(member);
    }

    public async Task UpdateAsync(int memberId, UpdateMemberDto dto) {
        var member = await _repository.GetByIdAsync(memberId);
        if (member == null) {
            _logger.LogWarning("Member {MemberId} not found for update", memberId);
            throw new KeyNotFoundException("Member not found");
        }

        if (!string.IsNullOrWhiteSpace(dto.Fname))
            member.Fname = dto.Fname;

        if (!string.IsNullOrWhiteSpace(dto.Lname))
            member.Lname = dto.Lname;

        if (!string.IsNullOrWhiteSpace(dto.Phone))
            member.Phone = dto.Phone;

        if (dto.Dob.HasValue)
            member.Dob = dto.Dob;

        if (dto.LevelId.HasValue)
            member.LevelId = dto.LevelId.Value;

        await _repository.UpdateAsync(member);

        _logger.LogInformation("Member {MemberId} updated successfully", memberId);
    }

    private MemberDto MapToDto(Oasis.Models.Member member) {
        return new MemberDto {
            MemberId = member.MemberId,
            Fname = member.Fname,
            Lname = member.Lname,
            Email = member.Email,
            Phone = member.Phone,
            Dob = member.Dob,
            TeamName = member.Team?.Name,
            LevelId = member.Level?.LevelId,
            LevelName = member.Level?.Name
        };
    }
}