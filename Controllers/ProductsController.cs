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
    }
}