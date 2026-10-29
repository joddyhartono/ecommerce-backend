namespace Ecommerce.Api.Queries
{
    public static class SellerQueries
    {
        public const string qCreate = @"
            INSERT INTO sellers (user_id, name, description)
            SELECT @UserId, @Name, @Description
            WHERE NOT EXISTS (
                SELECT 1 FROM sellers WHERE user_id = @UserId
            )";

        public const string qGetProducts = @"
            SELECT id, category_id AS CategoryId, seller_id AS SellerId, name, description, price, image_url AS ImageUrl
            FROM products
            WHERE seller_id = @UserId AND row_column = 0
        ";

        public const string qDeleteProduct = @"
            UPDATE products
            SET row_column = 1
            WHERE id = @ProductId AND seller_id = @UserId
        ";
    }
}