using IdentityService.Models;

namespace IdentityService.Requests
{
    public record CreateUserRequest(string FullName,string Email,string Password,Role Role,Guid? DepartmentId);
}
