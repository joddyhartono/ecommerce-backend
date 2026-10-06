namespace Ecommerce.Api.Queries
{
    public static class ProductQueries
    {
        public const string qGetFeatured = "SELECT id, name, price, image_url AS ImageUrl FROM products LIMIT 8";
        public const string qGetProducts = "SELECT id, name, price, image_url AS ImageUrl FROM products WHERE (@CategoryId IS NULL OR category_id = @CategoryId)";
        public const string qGetProduct = "SELECT id, name, price, description, image_url AS ImageUrl FROM products WHERE id = @ProductId";
    }
}