namespace Ecommerce.Api.Queries
{
    public static class ProductQueries
    {
        public const string GetFeatured = "SELECT id, name, price, image_url AS ImageUrl FROM products LIMIT 8";
        public const string GetProducts = "SELECT id, name, price, image_url AS ImageUrl FROM products WHERE (@categoryId IS NULL OR category_id = @categoryId)";
        public const string GetProduct = "SELECT id, name, price, description, image_url AS ImageUrl FROM products WHERE id = @productId";
    }
}