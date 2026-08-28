namespace Ecommerce.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? MidtransOrderId { get; set; }
        public string Status { get; set; }
        public decimal GrossAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public string? PaymentType { get; set; }
        public string Address { get; set; }
        public List<OrderItem> Items { get; set; } = [];
    }
}