namespace Ecommerce.Api.Queries
{    
    public static class CartQueries
    {
        public const string GetCart = @"
            SELECT  id, 
                    user_id AS UserId, 
                    created_at AS CreatedAt, 
                    updated_at AS UpdatedAt
            FROM carts
            WHERE user_id = @UserId";

        public const string GetCartItems = @"
            SELECT  id, 
                    cart_id AS CartId, 
                    product_id AS ProductId, 
                    quantity,
                    price,
                    created_at AS CreatedAt,
                    updated_at AS UpdatedAt,
                    (price * quantity) as subtotal
            FROM cart_items
            WHERE cart_id = @CartId";

        public const string GetProduct = @"
            SELECT  id, 
                    category_id AS CategoryId, 
                    name, 
                    description,
                    price,
                    image_url AS ImageUrl,
                    created_at AS CreatedAt,
                    updated_at AS UpdatedAt 
            FROM products
            WHERE id = @ProductId";
    }
}