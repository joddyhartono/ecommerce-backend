using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Cart? GetCart(int userId);
    }
}