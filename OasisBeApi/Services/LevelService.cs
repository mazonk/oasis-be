using Oasis.DTOs.Level;
using Oasis.Repositories.Interfaces;
using Oasis.Services.Interfaces;

namespace Oasis.Services;

public class LevelService : ILevelService {
    private readonly ILevelRepository _repository;
    private readonly ILogger<LevelService> _logger;

    public LevelService(ILevelRepository repository, ILogger<LevelService> logger) {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<LevelDto>> GetAllAsync() {
        var levels = await _repository.GetAllAsync();
        return levels.Select(MapToDto).ToList();
    }

    public async Task<LevelDto> GetByIdAsync(int levelId) {
        var level = await _repository.GetByIdAsync(levelId);
        if (level == null) {
            _logger.LogWarning("Level {LevelId} not found", levelId);
            throw new KeyNotFoundException("Level not found");
        }

        return MapToDto(level);
    }

    private static LevelDto MapToDto(Oasis.Models.Level level) => new() {
        LevelId = level.LevelId,
        Name = level.Name,
        MinExp = level.MinExp
    };
}