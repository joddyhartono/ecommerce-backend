using System.Security.Claims;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly ILogger<SellerController> _logger;
        private readonly ISellerRepository _repository;

        public SellerController(ILogger<SellerController> logger, ISellerRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [Authorize]
        [HttpPost("open")]
        public IActionResult OpenShop([FromBody] Seller seller)
        {
            _logger.LogInformation("Open shop started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var created = _repository.Create(new Seller
                {
                    UserId = userId,
                    Name = seller.Name,
                    Description = seller.Description
                });
                
                if(created)
                {
                    return StatusCode(201, created);   
                } 
                else
                {
                    return BadRequest("You already have a shop");    
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while opening a shop");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            _logger.LogInformation("GetProducts started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }
                var products = _repository.GetProducts(userId);
                _logger.LogInformation("GetProducts success");
                return Ok(products);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while getting products");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpDelete ("products/{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            _logger.LogInformation("DeleteProduct started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }
                var deleted = _repository.DeleteProduct(productId, userId);
                if(deleted > 0)
                {
                    _logger.LogInformation("DeleteProduct success");
                    return NoContent();
                } 
                else
                {
                    return NotFound("Product not found");
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while deleting a product");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("products/{productId}")]
        public IActionResult GetProduct(int productId)
        {
            _logger.LogInformation("GetProduct started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }
                var product = _repository.GetProduct(userId, productId);
                if(product == null)
                {
                    return NotFound("Product not found");
                }
                _logger.LogInformation("GetProduct success");
                return Ok(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while getting a product");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPut("products/{productId}")]
        public IActionResult UpdateProduct(int productId, Product product)
        {
            _logger.LogInformation("UpdateProduct started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }
                var existing = _repository.GetProduct(userId, productId);
                if(existing == null)
                {
                    return NotFound("Product not found");
                }

                existing.CategoryId = product.CategoryId;
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.Image = product.Image;

                var updatedProduct = _repository.UpdateProduct(existing);
                _logger.LogInformation("UpdateProduct success");
                return Ok(updatedProduct);
            }
            catch (Exception e)
            {
                _logger.LogError(e , "An error occurred while updating a product");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}