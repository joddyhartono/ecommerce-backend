namespace Ecommerce.Api.Queries
{    
    public static class CartQueries
    {
        public const string qGetCart = @"
            SELECT  id, 
                    user_id AS UserId, 
                    created_at AS CreatedAt, 
                    updated_at AS UpdatedAt
            FROM carts
            WHERE user_id = @UserId";

        public const string qGetCartItems = @"
            SELECT  id, 
                    cart_id AS CartId, 
                    product_id AS ProductId, 
                    quantity,
                    price,
                    created_at AS CreatedAt,
                    updated_at AS UpdatedAt,
                    (price * quantity) AS subtotal
            FROM cart_items
            WHERE cart_id = @CartId";

        public const string qGetProduct = @"
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

        public const string qAddToCart = @"
            INSERT INTO cart_items (cart_id, product_id, quantity, price)
            VALUES (@CartId, @ProductId, 1, @Price)
            RETURNING   id, 
                        cart_id AS CartId, 
                        product_id AS ProductId, 
                        quantity, 
                        price, 
                        price AS subtotal, 
                        created_at AS CreatedAt, 
                        updated_at AS UpdatedAt";

        public const string qRemoveFromCart = @"
            DELETE FROM cart_items
            WHERE cart_id = @CartId AND id = @Id";

    }
}