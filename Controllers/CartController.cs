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
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartController(ILogger<CartController> logger, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCart()
        {
            _logger.LogInformation("GetCart started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var cart = _cartRepository.GetCart(userId);
                return Ok(cart);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching cart");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{productId}")]
        [Authorize]
        public IActionResult AddToCart([FromRoute] int productId)
        {
            _logger.LogInformation("Add to started");
            try
            {
                var product = _productRepository.GetProduct(productId);
                if(product == null)
                {
                    return NotFound("Product not found");
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var cart = _cartRepository.GetCart(userId);
                var cartItem = _cartRepository.AddToCart(cart.Id, productId, product.Price);
                return Ok(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while adding an item to cart");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}