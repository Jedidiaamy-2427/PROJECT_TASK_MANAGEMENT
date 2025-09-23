using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Dtos.Auth;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
        {
            try
            {
                var resp = await _authService.RegisterAsync(request, ct);
                return Ok(resp);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
        {
            var resp = await _authService.LoginAsync(request, ct);
            if (resp is null)
            {
                return NotFound(new
                {
                    Title = "Utilisateur non trouvé ou mot de passe incorrect",
                    TraceID = HttpContext.TraceIdentifier
                });
            }
            return Ok(resp);
        }
    }
}


