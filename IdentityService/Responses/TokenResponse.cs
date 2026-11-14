namespace IdentityService.Responses
{
    public record TokenResponse(string AccessToken, string RefreshToken, DateTime Expires);
}
