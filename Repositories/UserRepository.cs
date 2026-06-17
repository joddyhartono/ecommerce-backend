using Dapper;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;

namespace Ecommerce.Api.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            
        }

        public User? GetUserByEmail(string email)
        {
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault<User>(UserQueries.GetByEmail, new
                {
                    Email = email
                });
            }
        }
    }
}