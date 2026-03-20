using Microsoft.EntityFrameworkCore;
using Oasis.Data;
using Oasis.Models;
using Oasis.Repositories.Interfaces;

namespace Oasis.Repositories;

public class LevelRepository : ILevelRepository {
    private readonly AppDbContext _context;

    public LevelRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Level?> GetHighestQualifyingLevelAsync(int experience) {
        return await _context.Levels
            .Where(l => l.MinExp <= experience)
            .OrderByDescending(l => l.MinExp)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Level>> GetAllAsync() {
        return await _context.Levels
            .OrderBy(l => l.MinExp)
            .ToListAsync();
    }

    public async Task<Level?> GetByIdAsync(int levelId) {
        return await _context.Levels.FindAsync(levelId);
    }
}