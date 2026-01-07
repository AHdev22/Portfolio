using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(AdminUser user);
    }
}