using Dapper;
using Ecommerce.Api.Models;
using Ecommerce.Api.Queries;
using Ecommerce.Api.Repositories.Interfaces;

namespace Ecommerce.Api.Repositories
{
    public class CartRepository : RepositoryBase, ICartRepository
    {
        public CartRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Cart? GetCart(int userId)
        {
            using(var connection = CreateConnection())
            {
                var cart = connection.QueryFirstOrDefault<Cart>(CartQueries.GetCart, new { UserId = userId });
                if(cart == null)
                {
                    return null;
                }
                var cartItems = connection.Query<CartItem>(CartQueries.GetCartItems, new { CartId = cart.Id });
                foreach (var cartItem in cartItems)
                {
                    var product = connection.QueryFirstOrDefault<Product>(CartQueries.GetProduct, new { ProductId = cartItem.ProductId });
                    cartItem.Product = product;
                }
                cart.Items = cartItems.ToList();
                return cart;
            }
        }
    }
}