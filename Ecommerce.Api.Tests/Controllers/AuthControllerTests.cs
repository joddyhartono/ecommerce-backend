using Castle.Core.Configuration;
using Castle.Core.Logging;
using Ecommerce.Api.Controllers;
using Ecommerce.Api.Helpers;
using Ecommerce.Api.Helpers.Interfaces;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Ecommerce.Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _repository;
        private readonly Mock<IJwtHelper> _jwtHelper;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _repository = new Mock<IUserRepository>();
            _jwtHelper = new Mock<IJwtHelper>();
            _controller = new AuthController(NullLogger<AuthController>.Instance, _repository.Object, _jwtHelper.Object);
        }

        [Fact]
        public void Login_EmailNotFound_Unauthorized()
        {
            // Arrange
            var user = new User
            {
                Email = "joddyhartono@gmail.com"
            };
            User existingUser = null;
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(existingUser);
            

            // Act
            var result = _controller.Login(user);

            // Assert
            var objectResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, objectResult.StatusCode);
            Assert.Equal("Invalid email or password", objectResult.Value);
        }

        [Fact]
        public void Login_WrongPassword_Unauthorized()
        {
            // Arrange
            var user = new User
            {
                Email = "joddyhartono@gmail.com",
                Password = "password"
            };
            var existingUser = new User
            {
                Email = "joddyhartono@gmail.com",
                Password = PasswordHelper.Hash("password123")
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(existingUser);

            // Act
            var result = _controller.Login(user);

            // Assert
            var objectResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, objectResult.StatusCode);
            Assert.Equal("Invalid email or password", objectResult.Value);
        }

        [Fact]
        public void Login_ThrowException()
        {
            var user = new User
            {
                Email = "joddyhartono@gmail.com"
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Throws(new Exception());

            // Act
            var result = _controller.Login(user);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public void Login_Success()
        {
            // Arrange
            var user = new User
            {
                Email = "joddyhartono@gmail.com",
                Password = "password"
            };
            var existingUser = new User
            {
                Email = "joddyhartono@gmail.com",
                Password = PasswordHelper.Hash("password")
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(existingUser);
            _jwtHelper.Setup(x => x.GenerateToken(user)).Returns("token");

            // Act
            var result = _controller.Login(user);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public void Register_EmailAlreadyExists_BadRequest()
        {
            // Arrange
            var user = new User
            {
                Email = "joddyhartono@gmail.com"
            };
            var existingUser = new User
            {
                Email = "joddyhartono@gmail.com"
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(existingUser);

            // Act
            var result = _controller.Register(user);

            // Arrange
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            Assert.Equal("Email already exists", objectResult.Value);
        }

        [Fact]
        public void Register_PasswordLengthLessThanEight_BadRequest()
        {
            // Arrange
            var user = new User
            {
                Email = "joddyhartono@gmail.com",
                Password = "joddy"
            };
            User existingUser = null;
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(existingUser);

            // Act
            var result = _controller.Register(user);

            // Arrange
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            Assert.Equal("Password must be at least 8 characters", objectResult.Value);
        }

        [Fact]
        public void Register_ThrowException()
        {
            var user = new User
            {
                Email = "joddyhartono@gmail.com"
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Throws(new Exception());

            // Act
            var result = _controller.Register(user);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public void Register_Success()
        {
            // Arrange
            var user = new User
            {
                Email = "joddyhartono@gmail.com",
                Password = "password"
            };
            User existingUser = null;
            var createdUser = new User
            {
                Id = 1,
                Name = "Joddy",
                Email = "joddyhartono@gmail.com",
                Image = null,
                IsSeller = false
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(existingUser);
            _repository.Setup(x => x.Insert(user)).Returns(createdUser);
            _jwtHelper.Setup(x => x.GenerateToken(user)).Returns("token");

            // Act
            var result = _controller.Register(user);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);
        }
    }
}