using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface ITaskItemService
    {
        Task<List<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TaskItem>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
        Task<TaskItem> CreateAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(int id, int isCompleted, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task ToggleCompleteAsync(int id, CancellationToken cancellationToken = default);
    }
}


