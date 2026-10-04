using Dapper;
using Ecommerce.Api.Models;
using Ecommerce.Api.Queries;
using Ecommerce.Api.Repositories.Interfaces;

namespace Ecommerce.Api.Repositories
{
    public class SellerRepository : RepositoryBase, ISellerRepository
    {
        public SellerRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public bool Create(Seller seller)
        {
            using (var connection = CreateConnection())
            {
                if(connection.Execute(SellerQueries.qCreate, seller) > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}