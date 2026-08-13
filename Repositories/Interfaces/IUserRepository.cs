using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        User? Update(User user);
    }
}