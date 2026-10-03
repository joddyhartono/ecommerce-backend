using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MidtransController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public MidtransController(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        [HttpPost("notification")]
        public async Task<IActionResult> HandleNotification([FromBody] MidtransNotification notification)
        {
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