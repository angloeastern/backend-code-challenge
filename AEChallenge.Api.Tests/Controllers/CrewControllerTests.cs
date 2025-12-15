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
    public class CrewControllerTests
    {
        private readonly Mock<ICrewRepository> _mockRepository;
        private readonly CrewController _controller;

        public CrewControllerTests()
        {
            _mockRepository = new Mock<ICrewRepository>();
            var mockLogger = new Mock<ILogger<CrewController>>();
            _controller = new CrewController(_mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public async Task GetCrewList_WithValidParameters_ShouldReturnOk()
        {
            // Arrange
            var parameters = new CrewListParameters
            {
                ShipCode = "SHIP001",
                PageNumber = 1,
                PageSize = 10
            };

            var crewMembers = new List<CrewListReadModel>
            {
                new() {
                    CrewMemberId = "CREW001",
                    FirstName = "John",
                    LastName = "Doe",
                    RankName = "Master"
                }
            };

            var expectedResponse = ApiResponseResult<IEnumerable<CrewListReadModel>>.Success(crewMembers);

            _mockRepository
                .Setup(x => x.GetCrewListAsync(It.IsAny<CrewListParameters>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetCrewList(parameters);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCrewList_WithNoResults_ShouldReturnNotFound()
        {
            // Arrange
            var parameters = new CrewListParameters
            {
                ShipCode = "SHIP001",
                PageNumber = 1,
                PageSize = 10
            };

            var emptyResponse = ApiResponseResult<IEnumerable<CrewListReadModel>>.Success(
                new List<CrewListReadModel>()
            );

            _mockRepository
                .Setup(x => x.GetCrewListAsync(It.IsAny<CrewListParameters>()))
                .ReturnsAsync(emptyResponse);

            // Act
            var result = await _controller.GetCrewList(parameters);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetCrewList_WithInvalidParameters_ShouldReturnBadRequest()
        {
            // Arrange
            var parameters = new CrewListParameters
            {
                ShipCode = "", // Invalid - empty
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _controller.GetCrewList(parameters);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetCrewList_WhenRepositoryFails_ShouldReturnBadRequest()
        {
            // Arrange
            var parameters = new CrewListParameters
            {
                ShipCode = "SHIP001",
                PageNumber = 1,
                PageSize = 10
            };

            var failureResponse = ApiResponseResult<IEnumerable<CrewListReadModel>>.Failure("Database error");

            _mockRepository
                .Setup(x => x.GetCrewListAsync(It.IsAny<CrewListParameters>()))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await _controller.GetCrewList(parameters);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
