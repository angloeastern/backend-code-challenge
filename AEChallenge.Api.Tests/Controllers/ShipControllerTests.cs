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
    public class ShipControllerTests
    {
        private readonly Mock<IShipRepository> _mockRepository;
        private readonly ShipController _controller;

        public ShipControllerTests()
        {
            _mockRepository = new Mock<IShipRepository>();
            var mockLogger = new Mock<ILogger<ShipController>>();
            _controller = new ShipController(_mockRepository.Object, mockLogger.Object);
        }

        #region CreateShip Tests

        [Fact]
        public async Task CreateShip_WithValidData_ShouldReturnOk()
        {

            var shipDto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "Active"
            };

            var expectedResponse = ApiResponseResult<ShipReadModel>.Success(
                new ShipReadModel
                {
                    ShipCode = "SHIP001",
                    ShipName = "Test Ship",
                    FiscalYear = 112,
                    Status = "Active"
                }
            );

            _mockRepository
                .Setup(x => x.AddShipsync(It.IsAny<ShipCreateDto>()))
                .ReturnsAsync(expectedResponse);


            var result = await _controller.CreateShip(shipDto);


            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task CreateShip_WithInvalidData_ShouldReturnBadRequest()
        {

            var shipDto = new ShipCreateDto
            {
                ShipCode = "",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "Active"
            };

            // Act
            var result = await _controller.CreateShip(shipDto);
            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateShip_WhenRepositoryFails_ShouldReturnBadRequest()
        {
            // Arrange
            var shipDto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "Active"
            };

            var failureResponse = ApiResponseResult<ShipReadModel>.Failure("Database error");

            _mockRepository
                .Setup(x => x.AddShipsync(It.IsAny<ShipCreateDto>()))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await _controller.CreateShip(shipDto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region GetShipList Tests

        [Fact]
        public async Task GetShipList_WithValidParameters_ShouldReturnOk()
        {
            // Arrange
            var parameters = new ShipListParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            var ships = new List<ShipReadModel>
            {
                new() { ShipCode = "SHIP001", ShipName = "Ship 1", FiscalYear = 112, Status = "Active" },
                new() { ShipCode = "SHIP002", ShipName = "Ship 2", FiscalYear = 403, Status = "Active" }
            };

            var expectedResponse = ApiResponseResult<IEnumerable<ShipReadModel>>.Success(ships);

            _mockRepository
                .Setup(x => x.GetShipAsync(It.IsAny<ShipListParameters>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetShipList(parameters);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var response = okResult!.Value as ApiResponseResult<IEnumerable<ShipReadModel>>;
            response!.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetShipList_WithNoResults_ShouldReturnNotFound()
        {
            // Arrange
            var parameters = new ShipListParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            var emptyResponse = ApiResponseResult<IEnumerable<ShipReadModel>>.Success(
                new List<ShipReadModel>()
            );

            _mockRepository
                .Setup(x => x.GetShipAsync(It.IsAny<ShipListParameters>()))
                .ReturnsAsync(emptyResponse);

            // Act
            var result = await _controller.GetShipList(parameters);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetShipList_WithInvalidParameters_ShouldReturnBadRequest()
        {
            // Arrange
            var parameters = new ShipListParameters
            {
                PageNumber = 0, // Invalid
                PageSize = 10
            };

            // Act
            var result = await _controller.GetShipList(parameters);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region CreateShipAssignment Tests

        [Fact]
        public async Task CreateShipAssignment_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var assignmentDto = new ShipAssignmentDto
            {
                UserId = 1,
                ShipCode = "SHIP001"
            };

            var expectedResponse = ApiResponseResult<ShipAssignmentReadModel>.Success(
                new ShipAssignmentReadModel
                {
                    UserId = 1,
                    ShipCode = "SHIP001",
                    UserName = "TestUser",
                    ShipName = "Test Ship"
                }
            );

            _mockRepository
                .Setup(x => x.AssignShipToUserAsync(It.IsAny<ShipAssignmentDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateShipAssignment(assignmentDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateShipAssignment_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var assignmentDto = new ShipAssignmentDto
            {
                UserId = 0, // Invalid
                ShipCode = "SHIP001"
            };

            // Act
            var result = await _controller.CreateShipAssignment(assignmentDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region GetShipsByUser Tests

        [Fact]
        public async Task GetShipsByUser_WithValidUserId_ShouldReturnOk()
        {
            // Arrange
            int userId = 1;

            var ships = new List<ShipAssignmentReadModel>
            {
                new() {
                    UserId = userId,
                    ShipCode = "SHIP001",
                    UserName = "TestUser",
                    ShipName = "Test Ship"
                }
            };

            var expectedResponse = ApiResponseResult<IEnumerable<ShipAssignmentReadModel>>.Success(ships);

            _mockRepository
                .Setup(x => x.GetAssignmentShipByUser(userId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateShipAssignment(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetShipsByUser_WithInvalidUserId_ShouldReturnBadRequest()
        {
            // Arrange
            int userId = 0;

            // Act
            var result = await _controller.CreateShipAssignment(userId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetShipsByUser_WhenRepositoryFails_ShouldReturnBadRequest()
        {
            // Arrange
            int userId = 1;

            var failureResponse = ApiResponseResult<IEnumerable<ShipAssignmentReadModel>>.Failure("User not found");

            _mockRepository
                .Setup(x => x.GetAssignmentShipByUser(userId))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await _controller.CreateShipAssignment(userId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion
    }
}
