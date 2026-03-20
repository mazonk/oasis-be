using Microsoft.Extensions.Logging;
using Oasis.DTOs.Member;
using Oasis.Repositories.Interfaces;
using Oasis.Services.Interfaces;

namespace Oasis.Services;

public class MemberService : IMemberService {
    private readonly IMemberRepository _repository;
    private readonly ILevelRepository _levelRepository;
    private readonly ILogger<MemberService> _logger;

    public MemberService(IMemberRepository repository, ILevelRepository levelRepository, ILogger<MemberService> logger) {
        _repository = repository;
        _levelRepository = levelRepository;
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

        if (dto.Experience.HasValue) {
            member.Experience = dto.Experience.Value;
            await CheckAndUpdateLevelAsync(member);
        }

        await _repository.UpdateAsync(member);

        _logger.LogInformation("Member {MemberId} updated successfully", memberId);
    }

    private async Task CheckAndUpdateLevelAsync(Oasis.Models.Member member) {
        var appropriateLevel = await _levelRepository.GetHighestQualifyingLevelAsync(member.Experience);
        if (appropriateLevel == null) return;
        if (member.LevelId == appropriateLevel.LevelId) return;

        member.LevelId = appropriateLevel.LevelId;
        _logger.LogInformation("Member {MemberId} leveled up to {LevelName}", member.MemberId, appropriateLevel.Name);
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
            Experience = member.Experience,
            LevelId = member.Level?.LevelId,
            LevelName = member.Level?.Name
        };
    }
}