using IdentityService.Models;
using System.Linq.Expressions;

namespace IdentityService.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct);
        Task<int> CountActiveAdminsAsync(CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
        Task<List<User>> GetAllUsersAsync(CancellationToken ct);
        Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken ct, bool track = false);
        Task<User?> GetFirstAsync(Expression<Func<User, bool>> predicate, CancellationToken ct, bool track = false);
        public Task RemoveUserAsync(User user, CancellationToken ct);
    }
}
