using Ecommerce.Api.Helpers;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MidtransController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public MidtransController(IConfiguration configuration, IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _configuration = configuration;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        [Authorize]
        [HttpPost("notification")]
        public async Task<IActionResult> HandleNotification([FromBody] MidtransNotification notification)
        {
            var serverKey = _configuration["Midtrans:ServerKey"];

            if (!MidtransHelper.IsValidSignature(notification, serverKey))
            {
                return Unauthorized("Invalid signature");
            }

            var order = _orderRepository.GetOrderByMidtransOrderId(notification.OrderId);
            if (order == null) return Ok();

            _orderRepository.UpdateOrderStatus(notification.OrderId, notification.TransactionStatus, notification.PaymentType);

            if (notification.TransactionStatus == "settlement")
            {
                _cartRepository.ClearCart(order.UserId);
            }

            return Ok();
        }
    }
}