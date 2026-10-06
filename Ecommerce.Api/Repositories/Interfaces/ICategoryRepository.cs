using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
    }
}