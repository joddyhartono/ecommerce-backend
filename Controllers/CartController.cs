using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartRepository _repository;

        public CartController(ILogger<CartController> logger, ICartRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCart()
        {
            _logger.LogInformation("GetCart started");
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var cart = _repository.GetCart(userId);
                return Ok(cart);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching cart");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}