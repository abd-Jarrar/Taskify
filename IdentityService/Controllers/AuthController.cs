using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Requests;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(AppDbContext context,IJwtTokenProvider jwtTokenProvider) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest request,CancellationToken ct)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user=context.Users.FirstOrDefault(u=>u.Email==email);

            if (user is null || !user.IsActive||user.PasswordHash!=request.Password)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            user.LastLoginAt = DateTime.Now;
            await context.SaveChangesAsync(ct);
            var token = jwtTokenProvider.GenerateJwtToken(
            new GenerateTokenRequest(user.Id, user.Email, user.Role, user.DepartmentId));

            return Ok(token);
        }
    }
}
