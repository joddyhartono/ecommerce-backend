using Dapper;
using Ecommerce.Api.Models;
using Ecommerce.Api.Queries;
using Ecommerce.Api.Repositories.Interfaces;

namespace Ecommerce.Api.Repositories
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Product> GetFeatured()
        {
            using(var connection = CreateConnection())
            {
                return connection.Query<Product>(ProductQueries.GetFeatured).ToList();
            }
        }
    }
}