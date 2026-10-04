using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface ISellerRepository
    {
        bool Create(Seller seller);
    }
}