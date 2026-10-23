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

        public List<Product> GetFeatured(int userId)
        {
            using(var connection = CreateConnection())
            {
                return connection.Query<Product, Seller, Product>(
                    ProductQueries.qGetFeatured,
                    (product, seller) =>
                    {
                        product.Seller = seller;
                        return product;
                    },
                    new { UserId = userId },
                    splitOn: "id"
                ).ToList();
            }
        }

        public List<Product> GetProducts(int? categoryId, int userId)
        {
            using(var connection = CreateConnection())
            {
                return connection.Query<Product, Seller, Product>(
                    ProductQueries.qGetProducts, 
                    (product, seller) =>
                    {
                        product.Seller = seller;
                        return product;
                    },
                    new {
                        CategoryId = categoryId,
                        UserId = userId
                    },
                    splitOn: "id"
                    ).ToList();
            }
        }

        public Product? GetProduct(int productId)
        {
            using(var connection = CreateConnection())
            {
                return connection.Query<Product, Seller, Product>(
                    ProductQueries.qGetProduct,
                    (product, seller) =>
                    {
                        product.Seller = seller;
                        return product;
                    },    
                    new
                    {
                        productId = productId
                    },
                    splitOn: "id"
                ).FirstOrDefault();
            }
        }
    }
}