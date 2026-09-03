using Dapper;
using Ecommerce.Api.Models;
using Ecommerce.Api.Queries;
using Ecommerce.Api.Repositories.Interfaces;

namespace Ecommerce.Api.Repositories
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
            
        }

        public Order CreateOrderWithItems(Order order, List<OrderItem> items)
        {
            using (var connection = CreateConnection())
            {
                order.Id = connection.QuerySingleOrDefault<int>(OrderQueries.qCreateOrder, order);
                foreach (var item in items)
                {
                    item.OrderId = order.Id;
                    connection.Execute(OrderQueries.qAddOrderItem, item);
                }
                return order;
            }
        }

        public Order GetOrderByMidtransOrderId(string midtransOrderId)
        {
            using (var connection = CreateConnection())
            {
                var order = connection.QuerySingleOrDefault<Order>(OrderQueries.qGetOrderByMidtransOrderId, new { MidtransOrderId = midtransOrderId});
                if(order == null)
                {
                    return null;
                }
                return order;
            }
        }

        public void UpdateOrderStatus(string orderId, string transactionStatus, string paymentType)
        {
            using (var connection = CreateConnection())
            {
                connection.Execute(OrderQueries.qUpdateOrderStatus, new { MidtransOrderId = orderId, TransactionStatus = transactionStatus, PaymentType = paymentType });
            }
        }
    }
}