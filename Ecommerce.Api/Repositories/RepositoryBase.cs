using Npgsql;

namespace Ecommerce.Api.Repositories
{
    public class RepositoryBase
    {
        private readonly IConfiguration _configuration;

        public RepositoryBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}