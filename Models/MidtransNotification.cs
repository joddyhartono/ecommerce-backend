namespace Ecommerce.Api.Models
{
    public class MidtransNotification
    {
        public string OrderId { get; set; }
        public string StatusCode { get; set; }
        public string GrossAmount { get; set; }
        public string TransactionStatus { get; set; }
        public string PaymentType { get; set; }
        public string SignatureKey { get; set; }
        public string FraudStatus { get; set; }
    }
}