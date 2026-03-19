using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oasis.Repositories.Interfaces;
using Oasis.Data;
using Oasis.Models;

namespace Oasis.Repositories;

public class AuthRepository : IAuthRepository {
    private readonly AppDbContext _context;
    private readonly ILogger<AuthRepository> _logger;

    public AuthRepository(AppDbContext context, ILogger<AuthRepository> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetUserByEmailAsync(string email) {
        try {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error fetching user by email: {Email}", email);
            throw;
        }
    }

    public async Task AddUserAsync(User user) {
        try {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error adding user: {Email}", user.Email);
            throw;
        }
    }

    public async Task<Member> AddMemberAsync(Member member) {
        try {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
            return member;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error adding member: {Email}", member.Email);
            throw;
        }
    }
}