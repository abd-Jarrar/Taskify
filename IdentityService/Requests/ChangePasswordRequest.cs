using System.ComponentModel.DataAnnotations;

namespace IdentityService.Requests
{
    public record ChangePasswordRequest( string OldPassword,string NewPassword);
}
