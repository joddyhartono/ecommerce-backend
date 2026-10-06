using System.Text.Json.Serialization;

namespace Ecommerce.Api.Models
{
    public class MidtransNotification
    {
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("status_code")]
        public string StatusCode { get; set; }

        [JsonPropertyName("gross_amount")]
        public string GrossAmount { get; set; }

        [JsonPropertyName("transaction_status")]
        public string TransactionStatus { get; set; }

        [JsonPropertyName("payment_type")]
        public string PaymentType { get; set; }

        [JsonPropertyName("signature_key")]
        public string SignatureKey { get; set; }

        [JsonPropertyName("fraud_status")]
        public string FraudStatus { get; set; }
    }
}