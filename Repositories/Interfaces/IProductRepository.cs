using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetFeatured();
    }
}