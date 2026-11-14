using IdentityService.Interfaces;
using IdentityService.Requests;
using IdentityService.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Services
{
    public class JwtTokenProvider(IConfiguration configuration): IJwtTokenProvider
    {
        public TokenResponse GenerateJwtToken(GenerateTokenRequest request)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("role", request.Role.ToString())
        };

            if (request.DepartmentId.HasValue)
                claims.Add(new Claim("departmentId", request.DepartmentId.Value.ToString()));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var minutes = int.Parse(configuration["Jwt:ExpiryMinutes"] ?? "60");
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(minutes);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials);

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken="1234",
                Expires = expires
            };
        }
    }
}
