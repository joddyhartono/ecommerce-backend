namespace Ecommerce.Api.Queries
{
    public static class OrderQueries
    {
        public const string qCreateOrder = @"
        INSERT INTO orders (user_id, midtrans_order_id, status, gross_amount, payment_type, address)
        VALUES (@UserId, @MidtransOrderId, @Status, @GrossAmount, @PaymentType, @Address)
        RETURNING id;
        ";

        public const string qAddOrderItem = @"
        INSERT INTO order_items (order_id, product_id, price, quantity)
        VALUES (@OrderId, @ProductId, @Price, @Quantity);
        ";

        public const string qGetOrderByMidtransOrderId = @"
        SELECT id, user_id AS UserId, midtrans_order_id AS MidtransOrderId, status, gross_amount AS GrossAmount, created_at AS CreatedAt, updated_at AS UpdatedAt, payment_type AS PaymentType, address
        FROM orders
        WHERE midtrans_order_id = @MidtransOrderId
        ";

        public const string qUpdateOrderStatus = @"
        UPDATE orders
        SET status = @TransactionStatus, payment_Type = @PaymentType
        WHERE midtrans_order_id = @MidtransOrderId
        ";
    }
}