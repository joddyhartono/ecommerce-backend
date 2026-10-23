using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetFeatured(int userId);
        List<Product> GetProducts(int? categoryId, int userId);
        Product? GetProduct(int productId);
    }
}