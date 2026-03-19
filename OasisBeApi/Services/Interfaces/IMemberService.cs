using Oasis.DTOs.Member;

namespace Oasis.Services.Interfaces;

public interface IMemberService {
    Task<MemberDto> GetByIdAsync(int memberId);
    Task<MemberDto> GetByEmailAsync(string email);
    Task UpdateAsync(int memberId, UpdateMemberDto dto);
}