using Dapper;
using Ecommerce.Api.Models;
using Ecommerce.Api.Queries;
using Ecommerce.Api.Repositories.Interfaces;

namespace Ecommerce.Api.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        {
            
        }

        public List<Category> GetAll()
        {
            using(var connection = CreateConnection())
            {
                return connection.Query<Category>(CategoryQueries.GetAll).ToList();
            }
        }
    }
}