using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Order CreateOrderWithItems(Order order, List<OrderItem> items);
        Order GetOrderByMidtransOrderId(string midtransOrderId);
        void UpdateOrderStatus(string orderId, string transactionStatus, string paymentType);
    }
}