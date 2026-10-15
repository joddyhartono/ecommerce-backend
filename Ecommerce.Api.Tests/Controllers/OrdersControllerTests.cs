using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Castle.Core.Logging;
using Ecommerce.Api.Controllers;
using Ecommerce.Api.Models;
using Ecommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Ecommerce.Api.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderRepository> _repository;

        public OrdersControllerTests()
        {
            _repository = new Mock<IOrderRepository>();
        }

        private OrdersController CreateController(string? userId)
        {
            var controller = new OrdersController(NullLogger<OrdersController>.Instance, _repository.Object);
            var claims = new List<Claim>();
            if(userId != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }
            var identity = new ClaimsIdentity(claims);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
            return controller;
        }
        
        [Fact]
        public void GetOrders_ThrowException()
        {
            // Arrange
            _repository.Setup(x => x.GetOrders(1)).Throws(new Exception());
            var controller = CreateController("1");

            // Act
            var result = controller.GetOrders();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public void GetOrders_Unauthorized()
        {
            // Arrange
            var controller = CreateController(null);

            // Act
            var result = controller.GetOrders();

            // Assert
            var objectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, objectResult.StatusCode);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public void GetOrders_Success(string userId)
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Id = int.Parse(userId),
                    UserId = int.Parse(userId),
                    MidtransOrderId = "Midtrans " + userId,
                    Status = "settlement",
                    GrossAmount = 800000,
                    Address = "Home",
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = 1,
                            OrderId = int.Parse(userId),
                            ProductId = 1,
                            Price = 800000,
                            Quantity = 1,
                            Subtotal = 800000,
                            Product = new Product
                            {
                                Id = 1,
                                CategoryId = 1,
                                Name = "Product",
                                Description = "Description",
                                Price = 800000,
                                ImageUrl = ""
                            }
                        }
                    }
                }
            };
            _repository.Setup(x => x.GetOrders(int.Parse(userId))).Returns(orders);
            var controller = CreateController(userId);

            // Act
            var result = controller.GetOrders();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(orders, objectResult.Value);
        }

        [Fact]
        public void GetOrderById_ThrowException()
        {
            // Arrange
            _repository.Setup(x => x.GetOrderById(1, 1)).Throws(new Exception());
            var controller = CreateController("1");

            // Act
            var result = controller.GetOrderById(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }

        [Fact]
        public void GetOrderById_Unauthorized()
        {
            // Arrange
            var controller = CreateController(null);

            // Act
            var result = controller.GetOrderById(1);

            // Assert
            var objectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, objectResult.StatusCode);
        }
        
        [Fact]
        public void GetOrderById_NotFound()
        {
            // Arrange
            Order order = null;
            _repository.Setup(x => x.GetOrderById(1, 1)).Returns(order);
            var controller = CreateController("1");

            // Act
            var result = controller.GetOrderById(1);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.Equal("Order not found", objectResult.Value);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public void GetOrderById_Success(string userId, int id)
        {
            // Arrange
            var order = new Order
            {
                Id = int.Parse(userId),
                UserId = int.Parse(userId),
                MidtransOrderId = "Midtrans " + userId,
                Status = "settlement",
                GrossAmount = 800000,
                Address = "Home",
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = 1,
                        OrderId = int.Parse(userId),
                        ProductId = 1,
                        Price = 800000,
                        Quantity = 1,
                        Subtotal = 800000,
                        Product = new Product
                        {
                            Id = 1,
                            CategoryId = 1,
                            Name = "Product",
                            Description = "Description",
                            Price = 800000,
                            ImageUrl = ""
                        }
                    }
                }
            };
            _repository.Setup(x => x.GetOrderById(int.Parse(userId), id)).Returns(order);
            var controller = CreateController(userId);

            // Act
            var result = controller.GetOrderById(id);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(order, objectResult.Value);
        }
    }
}