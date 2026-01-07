using app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.Infrastructure.Configurations
{
    public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> builder)
        {
            // 1. Primary Key
            builder.HasKey(x => x.Id);

            // 2. Username Configuration
            builder.Property(x => x.Username)
                   .IsRequired()
                   .HasMaxLength(50); // كفاية جداً لاسم المستخدم

            // ضروري جداً: منع تكرار اسم المستخدم
            builder.HasIndex(x => x.Username)
                   .IsUnique();

            // 3. PasswordHash Configuration
            builder.Property(x => x.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(500); // الـ Hash بيكون طويل عادة

            // 4. CreatedAt
            builder.Property(x => x.CreatedAt)
                   .IsRequired();
        }
    }
}