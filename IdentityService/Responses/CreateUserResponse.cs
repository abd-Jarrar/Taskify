namespace IdentityService.Responses
{
    public record CreateUserResponse(
    Guid Id,
    string FullName,
    string Email,
    string Role,
    string DepartmentName,
    DateTimeOffset CreatedAt);
}
