using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    public class ProjectService(IProjectRepository repository) : IProjectService
    {
        private readonly IProjectRepository _repository = repository;

        public Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default)
            => _repository.GetAllAsync(cancellationToken);

        public Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repository.GetByIdAsync(id, cancellationToken);

        public Task<List<Project>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
            => _repository.GetByUserIdAsync(userId, cancellationToken);

        public Task<Project> CreateAsync(Project project, CancellationToken cancellationToken = default)
            => _repository.AddAsync(project, cancellationToken);

        public Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(project, cancellationToken);

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(id, cancellationToken);
    }
}


