using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace IdentityService.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task AddAsync(User user, CancellationToken ct)
        {
            await context.Users.AddAsync(user,ct);
        }
        public async Task<int> CountActiveAdminsAsync(CancellationToken ct)
        {
            return  await context.Users.Where(u => (u.Role==Role.ADMIN)&& u.IsActive).CountAsync(ct);
            
        }
        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        {
            return await context.Users.AnyAsync(u => u.Email == email, ct);
        }
        public Task<List<User>> GetAllUsersAsync(CancellationToken ct)
        {
            return context.Users.ToListAsync(ct);
        }
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var user = await context.Users.Include(u=>u.Department).FirstOrDefaultAsync(u => u.Id == id, ct);
            return user;

        }
        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await context.SaveChangesAsync(ct);
        }
        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken ct)
        {
            return await context.Users.Include(u => u.Department).AnyAsync(predicate, ct);
        }
        public async Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken ct, bool track = false)
        {
            if(track)
                return await context.Users.AsNoTrackingWithIdentityResolution().Include(u=>u.Department).Where(predicate).ToListAsync(ct);
            else
                return await context.Users.Include(u => u.Department).Where(predicate).ToListAsync(ct);
        }
        public async Task<User?> GetFirstAsync(Expression<Func<User, bool>> predicate, CancellationToken ct, bool track = false)
        {
            if(track)
                return await context.Users.Include(u => u.Department).FirstOrDefaultAsync(predicate, ct);
            else
                return await context.Users.Include(u => u.Department).AsNoTracking().FirstOrDefaultAsync(predicate, ct);

        }
        
        public async Task RemoveUserAsync(User user, CancellationToken ct)
        {
              context.Users.Remove(user);
        }
    }
}
