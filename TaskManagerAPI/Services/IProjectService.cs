using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Project>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<Project> CreateAsync(Project project, CancellationToken cancellationToken = default);
        Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}


