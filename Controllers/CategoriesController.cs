using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryRepository _repository;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("GetAll started");
                var categories = _repository.GetAll();
                _logger.LogInformation("GetAll success");
                return Ok(categories);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving all categories");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}