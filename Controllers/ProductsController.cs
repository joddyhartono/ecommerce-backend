using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _repository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [Authorize]
        [HttpGet("featured")]
        public IActionResult GetFeatured()
        {
            _logger.LogInformation("GetFeatured started");
            try
            {
                var featured = _repository.GetFeatured();
                _logger.LogInformation("GetFeatured success");
                return Ok(featured);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving all featured products");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetProducts([FromQuery] int? categoryId, string? search)
        {
            _logger.LogInformation("GetProducts started");
            try
            {
                var products = _repository.GetProducts(categoryId);
                if(!string.IsNullOrWhiteSpace(search))
                {
                    products = products.Where(product => product.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                _logger.LogInformation("GetProducts success");
                return Ok(products);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving all products");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            _logger.LogInformation("GetProduct started");
            _logger.LogInformation("{productId}", id);
            try
            {
                var product = _repository.GetProduct(id);
                _logger.LogInformation("GetProduct success");
                return Ok(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving a product");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}