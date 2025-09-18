using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    public class UserService(IUserRepository repository) : IUserService
    {
        private readonly IUserRepository _repository = repository;

        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
            => _repository.GetAllAsync(cancellationToken);

        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repository.GetByIdAsync(id, cancellationToken);

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => _repository.GetByEmailAsync(email, cancellationToken);

        public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
            => _repository.GetByUsernameAsync(username, cancellationToken);

        public Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
            => _repository.AddAsync(user, cancellationToken);

        public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
            => _repository.UpdateAsync(user, cancellationToken);

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
            => _repository.DeleteAsync(id, cancellationToken);
    }
}


