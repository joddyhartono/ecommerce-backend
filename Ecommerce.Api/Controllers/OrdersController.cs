using System.Security.Claims;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderRepository _repository;

        public OrdersController(ILogger<OrdersController> logger, IOrderRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            _logger.LogInformation("GetOrders started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }
                var orders = _repository.GetOrders(userId);
                _logger.LogInformation("GetOrders success");
                return Ok(orders);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while getting orders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            _logger.LogInformation("GetOrderById started");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }
                var order = _repository.GetOrderById(userId, id);
                if(order == null)
                {
                    return NotFound("Order not found");
                }
                _logger.LogInformation("GerOrderById success");
                return Ok(order);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while getting an order by id");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}