using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManagerAPI.Dtos.Auth;
using TaskManagerAPI.Models;
using TaskManagerAPI.Options;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
    }

    public class AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshRepo, IOptions<JwtOptions> jwtOptions) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRefreshTokenRepository _refreshRepo = refreshRepo;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            User? user = request.UsernameOrEmail.Contains('@')
                ? await _userRepository.GetByEmailAsync(request.UsernameOrEmail, cancellationToken)
                : await _userRepository.GetByUsernameAsync(request.UsernameOrEmail, cancellationToken);

            if (user is null) return null;
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;

            var token = GenerateJwtToken(user);
            var r = await IssueRefreshTokenAsync(user, cancellationToken);
            return new AuthResponse { Token = token, RefreshToken = r.Token, Username = user.Username, Email = user.Email };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingEmail is not null) throw new InvalidOperationException("Email déjà utilisé");
            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (existingUsername is not null) throw new InvalidOperationException("Nom d'utilisateur déjà utilisé");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user, cancellationToken);

            var token = GenerateJwtToken(user);
            var r = await IssueRefreshTokenAsync(user, cancellationToken);
            return new AuthResponse { Token = token, RefreshToken = r.Token, Username = user.Username, Email = user.Email };
        }

        public async Task<AuthResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var rt = await _refreshRepo.GetAsync(refreshToken, cancellationToken);
            if (rt is null || !rt.IsActive) return null;

            // fetch user
            var user = await _userRepository.GetByIdAsync(rt.UserId, cancellationToken);
            if (user is null) return null;

            // rotate token: invalidate current & issue new
            var newRt = await IssueRefreshTokenAsync(user, cancellationToken);
            await _refreshRepo.InvalidateAsync(rt, newRt.Token, cancellationToken);

            var newAccess = GenerateJwtToken(user);
            return new AuthResponse { Token = newAccess, RefreshToken = newRt.Token, Username = user.Username, Email = user.Email };
        }

        public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var rt = await _refreshRepo.GetAsync(refreshToken, cancellationToken);
            if (rt is null || !rt.IsActive) return;
            await _refreshRepo.InvalidateAsync(rt, null, cancellationToken);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<RefreshToken> IssueRefreshTokenAsync(User user, CancellationToken cancellationToken)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var rt = new RefreshToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtOptions.RefreshExpiresDays)
            };
            return await _refreshRepo.AddAsync(rt, cancellationToken);
        }
    }
}


