using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetFeatured();
        List<Product> GetProducts(int? categoryId);
        Product? GetProduct(int productId);
    }
}