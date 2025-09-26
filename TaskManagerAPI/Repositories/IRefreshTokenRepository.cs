using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken = default);
        Task InvalidateAsync(RefreshToken token, string? replacedBy = null, CancellationToken cancellationToken = default);
    }
}


