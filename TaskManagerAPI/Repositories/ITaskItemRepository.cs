using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TaskItem>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
        Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(int id, int isCompleted, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}


