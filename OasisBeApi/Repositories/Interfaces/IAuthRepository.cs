using Oasis.Models;

namespace Oasis.Repositories.Interfaces;

public interface IAuthRepository {
    Task<User?> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
    Task<Member> AddMemberAsync(Member member);
}