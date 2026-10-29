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
            WHERE seller_id = @UserId
        ";
    }
}