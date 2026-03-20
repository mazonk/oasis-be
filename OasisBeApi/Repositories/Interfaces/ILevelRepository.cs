using Oasis.Models;

namespace Oasis.Repositories.Interfaces;

public interface ILevelRepository {
    Task<Level?> GetHighestQualifyingLevelAsync(int experience);
    Task<List<Level>> GetAllAsync();
    Task<Level?> GetByIdAsync(int levelId);
}