namespace Ecommerce.Api.Queries
{
    public static class ProductQueries
    {
        public const string qGetFeatured = "SELECT p.id, p.name, p.price, p.image_url AS ImageUrl, s.id, s.user_id AS UserId, s.name, s.image FROM products AS p JOIN sellers AS s ON p.seller_id = s.id WHERE s.id != @UserId LIMIT 8";
        public const string qGetProducts = "SELECT p.id, p.name, p.price, p.image_url AS ImageUrl, s.id, s.user_id AS UserId, s.name, s.image FROM products AS p JOIN sellers AS s ON p.seller_id = s.id WHERE (@CategoryId IS NULL OR p.category_id = @CategoryId) AND s.id != @UserId";
        public const string qGetProduct = "SELECT p.id, p.name, p.price, p.description, p.image_url AS ImageUrl, s.id, s.user_id AS UserId, s.name, s.image FROM products AS p JOIN sellers AS s ON p.seller_id = s.id WHERE p.id = @ProductId";
    }
}