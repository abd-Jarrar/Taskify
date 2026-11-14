using IdentityService.Data.Configurations;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Department>Departments => Set<Department>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}
