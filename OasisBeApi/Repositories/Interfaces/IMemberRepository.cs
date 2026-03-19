using Oasis.Models;

namespace Oasis.Repositories.Interfaces;

public interface IMemberRepository {
    Task<Member?> GetByIdAsync(int id);
    Task<Member?> GetByEmailAsync(string email);
    Task UpdateAsync(Member member);
}