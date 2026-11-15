using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Requests;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return Unauthorized(new ProblemDetails
                {
                    Title="Failed to login",
                    Detail = "Invalid email or password" 
                });
            }
            user.LastLoginAt = DateTime.Now;
            await context.SaveChangesAsync(ct);
            var token = jwtTokenProvider.GenerateJwtToken(
            new GenerateTokenRequest(user.Id, user.Email, user.Role, user.DepartmentId));

            return Ok(token);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirst("sub")!.Value);

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
            if (user is null || !user.IsActive)
                return Unauthorized(
                    new ProblemDetails
                    {
                        Title = "Failed to update the password",
                        Detail = "you're not logged in"
                    });

            if (user.PasswordHash != request.OldPassword)
                return BadRequest(new ProblemDetails {
                    Title = "Failed to update the password",
                    Detail= "Old password is incorrect"
                });

            if (request.OldPassword == request.NewPassword)
                return BadRequest(new ProblemDetails 
                {
                    Title= "Failed to update the password",
                    Detail = "New password must be different from the old one" 
                });

            user.PasswordHash = request.NewPassword;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            await context.SaveChangesAsync(ct);

            return NoContent();
        }
    
    }
}
