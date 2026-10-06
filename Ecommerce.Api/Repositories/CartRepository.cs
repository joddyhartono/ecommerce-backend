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

        public CartItem? AddToCart(int cartId, int productId, decimal price)
        {
            using (var connection = CreateConnection()) 
            {
                var cartItem = connection.QueryFirstOrDefault<CartItem>(CartQueries.qAddToCart, new { CartId = cartId, ProductId = productId, Price = price });
                if(cartItem == null)
                {
                    return null;
                }
                var product = connection.QueryFirstOrDefault<Product>(CartQueries.qGetProduct, new { ProductId = productId });
                cartItem.Product = product;
                return cartItem;
            }
        }

        public void ClearCart(int userId)
        {
            using (var connection = CreateConnection())
            {
                connection.Execute(CartQueries.qClearCart, new { UserId = userId });
            }
        }

        public CartItem? DecrementQuantity(int cartId, int cartItemId)
        {
            using (var connection = CreateConnection())
            {
                var cartItem = connection.QueryFirstOrDefault<CartItem>(CartQueries.qDecrementQuantity, new { CartId = cartId, Id = cartItemId});
                if(cartItem == null)
                {
                    return null;
                }

                var product = connection.QueryFirstOrDefault<Product>(CartQueries.qGetProduct, new { ProductId = cartItem.ProductId });
                cartItem.Product = product;
                return cartItem;
            }
        }

        public Cart? GetCart(int userId)
        {
            using(var connection = CreateConnection())
            {
                var cart = connection.QueryFirstOrDefault<Cart>(CartQueries.qGetCart, new { UserId = userId });
                if(cart == null)
                {
                    return null;
                }
                var cartItems = connection.Query<CartItem>(CartQueries.qGetCartItems, new { CartId = cart.Id });
                foreach (var cartItem in cartItems)
                {
                    var product = connection.QueryFirstOrDefault<Product>(CartQueries.qGetProduct, new { ProductId = cartItem.ProductId });
                    cartItem.Product = product;
                }
                cart.Items = cartItems.ToList();
                return cart;
            }
        }

        public CartItem? IncrementQuantity(int cartId, int cartItemId)
        {
            using (var connection = CreateConnection())
            {
                var cartItem = connection.QueryFirstOrDefault<CartItem>(CartQueries.qIncrementQuantity, new { CartId = cartId, Id = cartItemId });
                if(cartItem == null)
                {
                    return null;
                }
                
                var product = connection.QueryFirstOrDefault<Product>(CartQueries.qGetProduct, new { ProductId = cartItem.ProductId });
                cartItem.Product = product;
                return cartItem;
            }
        }

        public bool RemoveFromCart(int cartId, int cartItemId)
        {
            using (var connection = CreateConnection())
            {
                var isDeleted = connection.Execute(CartQueries.qRemoveFromCart, new {CartId = cartId, Id = cartItemId});
                if(isDeleted == 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}