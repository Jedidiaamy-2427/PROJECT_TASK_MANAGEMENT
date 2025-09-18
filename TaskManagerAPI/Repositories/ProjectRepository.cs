using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class ProjectRepository(AppDbContext dbContext) : IProjectRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Projects.AsNoTracking()
                .Include(p => p.TaskItems)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Projects.AsNoTracking()
                .Include(p => p.TaskItems)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Project>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Projects.AsNoTracking()
                .Where(p => p.UserId == userId)
                .Include(p => p.TaskItems)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return project;
        }

        public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
        {
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.Projects.FindAsync([id], cancellationToken);
            if (existing is not null)
            {
                _dbContext.Projects.Remove(existing);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}


