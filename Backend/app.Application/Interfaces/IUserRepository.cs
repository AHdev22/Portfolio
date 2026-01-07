using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<AdminUser?> GetByUsernameAsync(string Username);

    }
}