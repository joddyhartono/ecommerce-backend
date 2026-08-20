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
            _logger.LogInformation("Get cart started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var cart = _cartRepository.GetCart(userId);
                _logger.LogInformation("Get cart success");
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
            _logger.LogInformation("Add to cart started");
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
                _logger.LogInformation("Add to cart success");
                return Ok(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while adding an item to cart");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{cartItemId}")]
        [Authorize]
        public IActionResult RemoveFromCart([FromRoute] int cartItemId)
        {
            _logger.LogInformation("Remove from cart started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var cart = _cartRepository.GetCart(userId);
                var deleted = _cartRepository.RemoveFromCart(cart.Id, cartItemId);
                _logger.LogInformation("Remove from cart success");
                return NoContent();            
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while removing an item to cart");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{cartItemId}/increment")]
        [Authorize]
        public IActionResult IncrementQuantity([FromRoute] int cartItemId)
        {
            _logger.LogInformation("Increment cart item started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var cart = _cartRepository.GetCart(userId);
                var cartItem = _cartRepository.IncrementQuantity(cart.Id, cartItemId);
                _logger.LogInformation("Increment cart item success");
                return Ok(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while incrementing a cart item");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{cartItemId}/decrement")]
        [Authorize]
        public IActionResult DecrementQuantity([FromRoute] int cartItemId)
        {
            _logger.LogInformation("Decrement cart item started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var cart = _cartRepository.GetCart(userId);
                var cartItem = _cartRepository.DecrementQuantity(cart.Id, cartItemId);
                _logger.LogInformation("Decrement cart item success");
                return Ok(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while decrementing a cart item");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}