using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Cart? GetCart(int userId);
        CartItem? AddToCart(int cartId, int productId, decimal price);
        bool RemoveFromCart(int cartId, int cartItemId);
        CartItem? IncrementQuantity(int cartId, int cartItemId);
        CartItem? DecrementQuantity(int cartId, int cartItemId);
        void ClearCart(int userId);
    }
}