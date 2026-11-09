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
            SELECT p.id, p.category_id AS CategoryId, p.seller_id AS SellerId, p.name, p.description, p.price, p.image
            FROM sellers AS s
            INNER JOIN products AS p ON s.id = p.seller_id
            WHERE s.user_id = @UserId AND row_column = 0
        ";

        public const string qDeleteProduct = @"
            UPDATE products AS p
            SET row_column = 1
            FROM sellers AS s
            WHERE p.seller_id = s.id AND p.id = @ProductId AND s.user_id = @UserId
        ";

        public const string qGetProduct = @"
            SELECT p.id, p.category_id AS CategoryId, p.seller_id AS SellerId, p.name, p.description, p.price, p.image
            FROM sellers AS s
            JOIN products AS p ON s.id = p.seller_id
            WHERE s.user_id = @UserId AND p.id = @ProductId AND row_column = 0
        ";

        public const string qUpdateProduct = @"
            UPDATE products
            SET category_id = @CategoryId, name = @Name, description = @Description, price = @Price, image = @Image
            WHERE id = @Id
            RETURNING id, category_id AS CategoryId, name, description, price, image
        ";

        public const string qCreateProduct = @"
            INSERT INTO products (category_id, name, description, price, image, seller_id)
            SELECT @CategoryId, @Name, @Description, @Price, @Image, s.id
            FROM sellers AS s
            WHERE s.user_id = @UserId
            RETURNING id, category_id AS CategoryId, name, description, price, image
        ";
    }
}