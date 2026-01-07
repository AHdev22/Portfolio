using app.Application.Interfaces;
using app.Domain.Entities;
using app.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace app.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminUser?> GetByUsernameAsync(string username)
        {
            return await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
