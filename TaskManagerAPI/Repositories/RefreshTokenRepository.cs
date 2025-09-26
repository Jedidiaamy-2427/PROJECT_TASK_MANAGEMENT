using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class RefreshTokenRepository(AppDbContext dbContext) : IRefreshTokenRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<RefreshToken> AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
        {
            _dbContext.RefreshTokens.Add(token);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return token;
        }

        public Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken = default)
        {
            return _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
        }

        public async Task InvalidateAsync(RefreshToken token, string? replacedBy = null, CancellationToken cancellationToken = default)
        {
            token.RevokedAtUtc = DateTime.UtcNow;
            token.ReplacedByToken = replacedBy;
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}


