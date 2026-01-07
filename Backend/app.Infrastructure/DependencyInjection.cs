using System.Text; 
using app.Application.Interfaces;
using app.Infrastructure.Authentication;
using app.Infrastructure.Persistence;
using app.Infrastructure.Repositories;
using app.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace app.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Database Setup
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            // 2. Register Repositories & Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddTransient<DbInitializer>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            // 3. JWT Configuration
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Key"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT Secret Key is missing in appsettings.json.");
            }

            services.AddAuthorization(); // Required for [Authorize] attributes

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ClockSkew = TimeSpan.Zero // Optional: Removes the default 5-minute buffer on token expiry
                    };
                });

            return services;
        }
    }
}