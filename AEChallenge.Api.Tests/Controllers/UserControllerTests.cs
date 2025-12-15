using AEChallenge.Api.Controllers;
using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AEChallenge.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public async Task CreateUser_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                UserName = "testuser",
                Password = "password123",
                UserRole = "User"
            };

            var expectedResponse = ApiResponseResult<UserCreationReadModel>.Success(
                new UserCreationReadModel
                {
                    UserId = 1,
                    Username = "testuser",
                    UserRole = "User"
                }
            );

            _mockRepository
                .Setup(x => x.AddUserAsync(It.IsAny<UserCreateDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateUser_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                UserName = "", // Invalid - empty
                Password = "password123",
                UserRole = "User"
            };

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetUsersList_WithValidParameters_ShouldReturnOk()
        {
            // Arrange
            var parameters = new BaseQueryParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            var users = new List<UserCreationReadModel>
            {
                new() { UserId = 1, Username = "user1", UserRole = "User" },
                new() { UserId = 2, Username = "user2", UserRole = "Admin" }
            };

            var expectedResponse = ApiResponseResult<IEnumerable<UserCreationReadModel>>.Success(users);

            _mockRepository
                .Setup(x => x.GetUserAsync(It.IsAny<BaseQueryParameters>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetUsersList(parameters);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetUsersList_WithNoResults_ShouldReturnNotFound()
        {
            // Arrange
            var parameters = new BaseQueryParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            var emptyResponse = ApiResponseResult<IEnumerable<UserCreationReadModel>>.Success(
                new List<UserCreationReadModel>()
            );

            _mockRepository
                .Setup(x => x.GetUserAsync(It.IsAny<BaseQueryParameters>()))
                .ReturnsAsync(emptyResponse);

            // Act
            var result = await _controller.GetUsersList(parameters);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateUser_WhenRepositoryFails_ShouldReturnBadRequest()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                UserName = "testuser",
                Password = "password123",
                UserRole = "User"
            };

            var failureResponse = ApiResponseResult<UserCreationReadModel>.Failure("Database error");

            _mockRepository
                .Setup(x => x.AddUserAsync(It.IsAny<UserCreateDto>()))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
