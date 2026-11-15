using IdentityService.Models;
using IdentityService.Responses;

namespace IdentityService.Common
{
    public static class ExtensionMethods
    {
        extension(User user)
        {
            public GetUserResponse ToGetUserResponse()
            {
                return new GetUserResponse(user.Id, user.FullName, user.Email, nameof(user.Role), user.Department!.Name, user.IsActive);
            }
            public CreateUserResponse ToCreateUserResponse()
            {
                return new CreateUserResponse(user.Id, user.FullName, user.Email, nameof(user.Role), user.Department.Name, DateTimeOffset.UtcNow);
            }
        }
    }
}
