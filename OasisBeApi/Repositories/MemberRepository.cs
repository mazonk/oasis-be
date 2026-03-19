using Microsoft.EntityFrameworkCore;
using Oasis.Models;
using Oasis.Repositories.Interfaces;
using Oasis.Data;

namespace Oasis.Repositories;

public class MemberRepository : IMemberRepository {
    private readonly AppDbContext _context;

    public MemberRepository(AppDbContext db) {
        _context = db;
    }

    public async Task<Member?> GetByIdAsync(int memberId) {
        return await _context.Members
            .Include(m => m.Team)
            .Include(m => m.Level)
            .FirstOrDefaultAsync(m => m.MemberId == memberId);
    }

    public async Task<Member?> GetByEmailAsync(string email) {
        return await _context.Members
            .Include(m => m.Team)
            .Include(m => m.Level)
            .FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task UpdateAsync(Member member) {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
    }
}