using Ecommerce.Api.Controllers;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Ecommerce.Api.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryRepository> _repository;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _repository = new Mock<ICategoryRepository>();
            _controller = new CategoriesController(NullLogger<CategoriesController>.Instance, _repository.Object);
        }

        [Fact]  
        public void GetAll_ThrowException()
        {
            // Arrange
            _repository.Setup(x => x.GetAll()).Throws(new Exception());
            
            // Act
            var result = _controller.GetAll();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }
    }   
}