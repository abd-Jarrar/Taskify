using IdentityService.Requests;
using IdentityService.Responses;

namespace IdentityService.Interfaces
{
    public interface IJwtTokenProvider
    {
        TokenResponse GenerateJwtToken(GenerateTokenRequest request);

    }
}
