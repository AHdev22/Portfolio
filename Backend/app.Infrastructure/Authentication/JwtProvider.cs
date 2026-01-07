using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using app.Application.Interfaces;
using app.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace app.Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;

        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate(AdminUser user)
        {
            // 1. Get Secret Key from AppSettings
            var secretKey = _configuration["JwtSettings:Key"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 2. Create Claims (بيانات المستخدم داخل التوكين)
            var claims = new[]
            {
                // هنا استخدمنا الـ Class من الـ Namespace الموحد
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // ID
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username), // Username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Token ID
            };

            // 3. Create Token Structure
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"]!)),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = credentials
            };

            // 4. Write Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}