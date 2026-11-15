using IdentityService.Common;
using IdentityService.Interfaces;
using IdentityService.Models;
using IdentityService.Requests;
using IdentityService.Responses;

namespace IdentityService.Services
{
    public class UserService(IUserRepository userRepository)
    {
        public async Task<Result<GetUserResponse>> GetByIdAsync(Guid id, CancellationToken ct) {
            var user = await userRepository.GetFirstAsync(u => u.Id == id, ct);

            if (user is null)
                return Result<GetUserResponse>.Failure(404, "the user was not found");
            else
                return Result<GetUserResponse>.Success(user.ToGetUserResponse());
        }
        public async Task<Result<List<GetUserResponse>>> GetAllAsync(CancellationToken ct)
        {
            var users = await userRepository.GetAllUsersAsync(ct);
            List<GetUserResponse> list = new();
            if (users.Count <= 0)
                return Result<List<GetUserResponse>>.Failure(404, "there is no users in the system");
            foreach(var user in users)
            {
                list.Add(user.ToGetUserResponse());
            }

            return Result<List<GetUserResponse>>.Success(list);
        }
        public async Task<Result<CreateUserResponse>> CreateAsync(CreateUserRequest request, CancellationToken ct)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (await userRepository.EmailExistsAsync(email, ct))
                return Result<CreateUserResponse>.Failure(StatusCodes.Status409Conflict,"Email is already in use");

            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = email,
                PasswordHash = request.Password,
                Role = request.Role,
                DepartmentId = request.DepartmentId,
                IsActive = true
            };

            await userRepository.AddAsync(user, ct);
            await userRepository.SaveChangesAsync(ct);

            return Result<CreateUserResponse>.Success(user.ToCreateUserResponse());
        }
        //public Task<Result<UserResponse>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct);
        //public Task<Result<UserResponse>> UpdateMyProfileAsync(Guid id, UpdateMyProfileRequest request, CancellationToken ct);
        //public Task<Result<UserResponse>> ChangeRoleAsync(Guid id, Role role, CancellationToken ct);
        //public Task<Result<UserResponse>> ChangeStatusAsync(Guid id, bool isActive, CancellationToken ct);
        //public Task<Result<bool>> DeleteAsync(Guid id, Guid currentUserId, CancellationToken ct);
    }
}
