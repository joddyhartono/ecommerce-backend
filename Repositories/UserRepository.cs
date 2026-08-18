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

        public User? GetByEmail(string email)
        {
            using(var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault<User>(UserQueries.qGetByEmail, new
                {
                    Email = email
                });
            }
        }

        public User? Update(User user)
        {
            using(var connection = CreateConnection())
            {
                connection.Execute(UserQueries.qUpdate, user);
                return connection.QueryFirstOrDefault<User>(UserQueries.qGetByEmail, new {Email = user.Email});
            }
        }
    }
}