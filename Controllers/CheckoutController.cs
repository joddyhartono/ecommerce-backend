using Ecommerce.Api.Helpers;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        
        public CheckoutController(ICartRepository cartRepository, IOrderRepository orderRepository, IConfiguration configuration)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout([FromBody] Checkout request)
        {
            string serverKey = _configuration["Midtrans:ServerKey"];
            bool isProduction = bool.Parse(_configuration["Midtrans:IsProduction"]);

            var cart = _cartRepository.GetCart(request.CartId);
            if(cart == null)
            {
                return NotFound("Cart not found");
            }

            if(string.IsNullOrWhiteSpace(request.Address))
            {
                return BadRequest("Address is required");
            }

            decimal grossAmount = cart.Items.Sum(item => item.Price * item.Quantity);

            var order = new Order
            {
                UserId = cart.UserId,
                MidtransOrderId = $"ORDER-{cart.UserId}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}", 
                Status = "pending",
                GrossAmount = grossAmount,
                Address = request.Address
            };

            var orderItems = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                orderItems.Add
                (
                    new OrderItem
                    {
                        ProductId = item.ProductId,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Product = item.Product
                    }
                );
            }

            order = _orderRepository.CreateOrderWithItems(order, orderItems);

            long midtransGrossAmount = (long)Math.Round(grossAmount, MidpointRounding.AwayFromZero);
            Midtrans response = await MidtransHelper.Snap(serverKey, isProduction, order.MidtransOrderId, midtransGrossAmount);
            return Ok(new
            {
                token = response.Token,
                orderId = order.Id
            });
        }
    }
}