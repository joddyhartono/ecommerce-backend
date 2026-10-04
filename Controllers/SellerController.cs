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
    }
}