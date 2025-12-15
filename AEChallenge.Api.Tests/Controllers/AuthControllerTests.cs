using AEChallenge.Api.Controllers;
using AEChallenge.Application.DTOs;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AEChallenge.Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthRepository = new Mock<IAuthRepository>();
            _mockJwtService = new Mock<IJwtService>();
            var mockLogger = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_mockAuthRepository.Object, _mockJwtService.Object, mockLogger.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOk()
        {
            // Arrange
            var loginDto = new UserLoginDto
            {
                Username = "testuser",
                Password = "password123"
            };

            var userLoginReadModel = new UserLoginReadModel
            {
                UserId = 1,
                Username = "testuser",
                UserRole = "User"
            };

            var authResponse = ApiResponseResult<UserLoginReadModel>.Success(userLoginReadModel);

            _mockAuthRepository
                .Setup(x => x.AuthenticateAsync(loginDto.Username, loginDto.Password))
                .ReturnsAsync(authResponse);

            _mockJwtService
                .Setup(x => x.GenerateToken(It.IsAny<UserLoginReadModel>()))
                .Returns("jwt-token-here");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var response = okResult!.Value as ApiResponseResult<TokenResponseDto>;
            response!.Data.Token.Should().Be("jwt-token-here");
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnBadRequest()
        {
            // Arrange
            var loginDto = new UserLoginDto
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            var failureResponse = ApiResponseResult<UserLoginReadModel>.Failure("Invalid credentials");

            _mockAuthRepository
                .Setup(x => x.AuthenticateAsync(loginDto.Username, loginDto.Password))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_WithInvalidDto_ShouldReturnBadRequest()
        {
            // Arrange
            var loginDto = new UserLoginDto
            {
                Username = "", // Invalid - empty
                Password = "password123"
            };

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ShouldReturnBadRequest()
        {
            // Arrange
            var loginDto = new UserLoginDto
            {
                Username = "testuser",
                Password = "" // Invalid - empty
            };

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
