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
        SET status = @TransactionStatus, payment_type = @PaymentType, updated_at = NOW()
        WHERE midtrans_order_id = @MidtransOrderId
        ";

        public const string qExpireUnpaidOrders = @"
        UPDATE orders
        SET status = 'expired', updated_at = NOW()
        WHERE status = 'pending' AND created_at < NOW() - INTERVAL '1 day'
        ";

        public const string qGetOrders = @"
        SELECT id, midtrans_order_id AS MidtransOrderId, status, gross_amount AS GrossAmount
        FROM orders
        WHERE user_id = @UserId
        ";

        public const string qGetOrderById = @"
        SELECT o.id, o.midtrans_order_id AS MidtransOrderId, 
        o.status, o.gross_amount AS GrossAmount,
        o.payment_type AS PaymentType, o.address,
        oi.id, oi.price, oi.quantity,
        p.id, p.name, p.price, p.image_url AS ImageUrl
        FROM orders as o
        JOIN order_items AS oi ON o.id = oi.order_id
        JOIN products as p on oi.product_id = p.id
        WHERE o.user_id = @UserId AND o.id = @OrderId
        ";
    }
}