using Oasis.DTOs.Level;

namespace Oasis.Services.Interfaces;

public interface ILevelService {
    Task<List<LevelDto>> GetAllAsync();
    Task<LevelDto> GetByIdAsync(int levelId);
}