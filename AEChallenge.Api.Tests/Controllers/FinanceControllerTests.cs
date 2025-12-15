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
    public class FinanceControllerTests
    {
        private readonly Mock<IFinanceRepository> _mockRepository;
        private readonly FinanceController _controller;

        public FinanceControllerTests()
        {
            _mockRepository = new Mock<IFinanceRepository>();
            var mockLogger = new Mock<ILogger<FinanceController>>();
            _controller = new FinanceController(_mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public async Task GetFinancialSummaryReport_WithValidParameters_ShouldReturnOk()
        {
            // Arrange
            var parameters = new FinancialReportParameters
            {
                ShipCode = "SHIP001",
                PeriodId = "202501"
            };

            var reports = new List<FinancialReportSummaryReadModel>
            {
                new() {
                    AccountDescription = "Operating Expenses",
                    AccountNumber = "7000000"
                }
            };

            var expectedResponse = ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.Success(reports);

            _mockRepository
                .Setup(x => x.GetFinancialSummaryReportAsync(parameters.ShipCode, parameters.PeriodId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetFinancialSummaryReport(parameters);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetFinancialDetailReport_WithValidParameters_ShouldReturnOk()
        {
            // Arrange
            var parameters = new FinancialReportParameters
            {
                ShipCode = "SHIP001",
                PeriodId = "202501"
            };

            var reports = new List<FinancialReportDetailReadModel>
            {
                new() {
                    AccountDescription = "Operating Expenses",
                    AccountNumber = "7000000"
                }
            };

            var expectedResponse = ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.Success(reports);

            _mockRepository
                .Setup(x => x.GetFinancialDetailReportAsync(parameters.ShipCode, parameters.PeriodId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetFinancialDetailReport(parameters);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetFinancialSummaryReport_WithNoResults_ShouldReturnNotFound()
        {
            // Arrange
            var parameters = new FinancialReportParameters
            {
                ShipCode = "SHIP001",
                PeriodId = "202501"
            };

            var emptyResponse = ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.Success(
                new List<FinancialReportSummaryReadModel>()
            );

            _mockRepository
                .Setup(x => x.GetFinancialSummaryReportAsync(parameters.ShipCode, parameters.PeriodId))
                .ReturnsAsync(emptyResponse);

            // Act
            var result = await _controller.GetFinancialSummaryReport(parameters);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetFinancialDetailReport_WithNoResults_ShouldReturnNotFound()
        {
            // Arrange
            var parameters = new FinancialReportParameters
            {
                ShipCode = "SHIP001",
                PeriodId = "202501"
            };

            var emptyResponse = ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.Success(
                new List<FinancialReportDetailReadModel>()
            );

            _mockRepository
                .Setup(x => x.GetFinancialDetailReportAsync(parameters.ShipCode, parameters.PeriodId))
                .ReturnsAsync(emptyResponse);

            // Act
            var result = await _controller.GetFinancialDetailReport(parameters);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetFinancialSummaryReport_WithInvalidParameters_ShouldReturnBadRequest()
        {
            // Arrange
            var parameters = new FinancialReportParameters
            {
                ShipCode = "", // Invalid - empty
                PeriodId = "202501"
            };

            // Act
            var result = await _controller.GetFinancialSummaryReport(parameters);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
