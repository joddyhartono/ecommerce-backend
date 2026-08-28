using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ecommerce.Api.Models;

namespace Ecommerce.Api.Helpers
{
    public static class MidtransHelper
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<Midtrans> Snap(string serverKey, bool isProduction, int orderId, decimal grossAmount)
        {
            var endpoint = isProduction ? "https://app.midtrans.com/snap/v1/transactions" : "https://app.sandbox.midtrans.com/snap/v1/transactions";
            
            var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes(serverKey + ":"));

            using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authString);

                var parameter = new
                {
                    transaction_details = new
                    {
                        order_id = orderId,
                        gross_amount = grossAmount
                    }
                };

                var json = JsonSerializer.Serialize(parameter);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request);
                var data = await response.Content.ReadAsStringAsync();

                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Midtrans error ({response.StatusCode}): ({data})");
                }

                var result = JsonSerializer.Deserialize<Midtrans>(data);
                return result;
            }
        }
    }
}