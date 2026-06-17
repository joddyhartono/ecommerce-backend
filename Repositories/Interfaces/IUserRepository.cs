using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByEmail(string email);
    }
}