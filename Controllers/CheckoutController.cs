using Ecommerce.Api.Helpers;
using Ecommerce.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout([FromBody] Cart cart)
        {
            string serverKey = _configuration["Midtrans:ServerKey"];
            bool isProduction = bool.Parse(_configuration["Midtrans:IsProduction"]);

            decimal grossAmount = cart.Items.Sum(item => item.Price * item.Quantity);
            Midtrans response = await MidtransHelper.Snap(serverKey, isProduction, cart.Id, grossAmount);
            return Ok(response);
        }
    }
}