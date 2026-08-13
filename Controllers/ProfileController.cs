using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IUserRepository _repository;

        public ProfileController (ILogger<ProfileController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        
        [Authorize]
        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            _logger.LogInformation("Update user started");
            try
            {
                var existingUser = _repository.GetByEmail(user.Email);
                if(existingUser == null)
                {
                    return NotFound("User not found");
                }
                var updatedUser = _repository.Update(user);
                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while updating user");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}