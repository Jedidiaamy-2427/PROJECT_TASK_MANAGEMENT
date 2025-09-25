using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class TaskItemRepository(AppDbContext dbContext) : ITaskItemRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<List<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.TaskItems.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TaskItems.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TaskItem>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TaskItems.AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            _dbContext.TaskItems.Add(task);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return task;
        }

        public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            _dbContext.TaskItems.Update(task);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateStatusAsync(int id, int isCompleted, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.TaskItems.FindAsync([id], cancellationToken);
            if (existing is not null)
            {
                existing.IsCompleted = isCompleted;
                _dbContext.TaskItems.Update(existing);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.TaskItems.FindAsync([id], cancellationToken);
            if (existing is not null)
            {
                _dbContext.TaskItems.Remove(existing);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}


