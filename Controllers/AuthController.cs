using Ecommerce.Api.Helpers;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _repository;
        private readonly JwtHelper _jwtHelper;


        public AuthController(ILogger<AuthController> logger, IUserRepository repository, JwtHelper jwtHelper)
        {
            _logger = logger;
            _repository = repository;
            _jwtHelper = jwtHelper;
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            _logger.LogInformation("Login started");
            try
            {
                var existingUser = _repository.GetUserByEmail(user.Email);
                if(existingUser == null)
                {
                    return Unauthorized("Invalid email or password");
                }

                if(!PasswordHelper.Verify(user.Password, existingUser.Password))
                {
                    return Unauthorized("Invalid email or password");
                }

                _logger.LogInformation("Login success");
                return Ok(new
                {
                    User = new
                    {
                        Id = existingUser.Id,
                        Name = existingUser.Name,
                        Email = existingUser.Email,
                        Image = existingUser.Image
                    },
                    Token = _jwtHelper.GenerateToken(existingUser)
                });             
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while logging in");                
                return StatusCode(500, "Internal server error");
            }
        }
    }
}