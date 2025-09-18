using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    public class TaskItemService(ITaskItemRepository repository) : ITaskItemService
    {
        private readonly ITaskItemRepository _repository = repository;

        public Task<List<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
            => _repository.GetAllAsync(cancellationToken);

        public Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repository.GetByIdAsync(id, cancellationToken);

        public Task<List<TaskItem>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
            => _repository.GetByProjectIdAsync(projectId, cancellationToken);

        public Task<TaskItem> CreateAsync(TaskItem task, CancellationToken cancellationToken = default)
            => _repository.AddAsync(task, cancellationToken);

        public Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(task, cancellationToken);

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(id, cancellationToken);

        public async Task ToggleCompleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing is null) return;
            existing.IsCompleted = !existing.IsCompleted;
            await _repository.UpdateAsync(existing, cancellationToken);
        }
    }
}


