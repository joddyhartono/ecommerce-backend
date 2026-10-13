using Ecommerce.Api.Controllers;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Ecommerce.Api.Tests.Controllers
{
    public class ProfileControllerTests
    {
        private readonly Mock<IUserRepository> _repository;
        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _repository = new Mock<IUserRepository>();
            _controller = new ProfileController(NullLogger<ProfileController>.Instance, _repository.Object);
        }

        [Fact]
        public void Update_ThrowException()
        {
            // Arrange
            User user = new User
            {
                Email = "joddyhartono@gmail.com"
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Throws(new Exception());

            // Act
            var result = _controller.Update(user);

            // Arrange
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public void Update_UserNotFound()
        {
            // Arrange
            User user = new User
            {
                Email = "joddyhartono@gmail.com"
            };
            User existingUser = null;
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(existingUser);

            // Act
            var result = _controller.Update(user);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public void Update_Success()
        {
            // Arrange
            User user = new User
            {
                Name = "Joddy",
                Email = "joddyhartono@gmail.com"
            };
            User updatedUser = new User
            {
                Name = "Joddy",
            };
            _repository.Setup(x => x.GetByEmail(user.Email)).Returns(user);
            _repository.Setup(x => x.Update(user)).Returns(updatedUser);

            // Act
            var result = _controller.Update(user);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(updatedUser, objectResult.Value);
        }
    }
}