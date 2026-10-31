using Ecommerce.Api.Models;

namespace Ecommerce.Api.Helpers.Interfaces
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
    }
}