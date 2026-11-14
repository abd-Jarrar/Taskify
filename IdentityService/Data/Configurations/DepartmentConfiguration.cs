using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("departments");
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.ManagerId)
                .IsUnique()
                .HasFilter("[ManagerId] IS NOT NULL");
            builder.HasOne(x => x.Manager)
             .WithMany()
             .HasForeignKey(x => x.ManagerId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
