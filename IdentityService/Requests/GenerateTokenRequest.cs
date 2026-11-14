using IdentityService.Models;

namespace IdentityService.Requests
{
    public record GenerateTokenRequest(Guid UserId, string Email, Role Role, Guid? DepartmentId);

}