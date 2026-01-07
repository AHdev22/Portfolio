using app.Domain.Entities;
using app.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace app.Infrastructure.Persistence
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public DbInitializer(AppDbContext context, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public void Seed()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration Error: {ex.Message}");
            }

            var adminUsername = _configuration["AdminSettings:Username"] ?? "admin";
            var adminPassword = _configuration["AdminSettings:Password"] ?? "123456";

            if (!_context.AdminUsers.Any())
            {
                var passwordHash = _passwordHasher.Hash(adminPassword);

                // استخدام الـ Constructor لأن الـ Setters نوعها private
                var adminUser = new AdminUser(
                    Guid.NewGuid(),
                    adminUsername,
                    passwordHash
                );

                _context.AdminUsers.Add(adminUser);
                _context.SaveChanges();
            }

        }
    }
}