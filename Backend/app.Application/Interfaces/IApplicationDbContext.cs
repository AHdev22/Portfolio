using app.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<AdminUser> AdminUsers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}