namespace IdentityService.Responses
{
    
    
    public record GetUserResponse(
        Guid Id,
        string FullName,
        string Email,
        string Role,
        string DepartmentName,
        bool IsActive);
}
