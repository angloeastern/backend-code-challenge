using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using AEChallenge.Domain.Entities;
using AEChallenge.Infrastructure.Data;
using AEChallenge.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AEChallenge.Infrastructure.Tests.Repositories
{
    public class ShipRepositoryTests
    {
        [Fact]
        public void ShipRepository_Constructor_ShouldInitializeCorrectly()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockCurrentUserService = new Mock<ICurrentUserService>();
            mockCurrentUserService.Setup(x => x.UserId).Returns("1");
            mockCurrentUserService.Setup(x => x.UserName).Returns("TestUser");
            var mockLogger = new Mock<ILogger<ShipRepository>>();


            var repository = new ShipRepository(context, mockCurrentUserService.Object, mockLogger.Object);


            repository.Should().NotBeNull();
            repository.Should().BeOfType<ShipRepository>();
        }

        [Fact]
        public void ShipRepository_ShouldImplementIShipRepository()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockLogger = new Mock<ILogger<ShipRepository>>();


            var repository = new ShipRepository(context, mockCurrentUserService.Object, mockLogger.Object);


            repository.Should().BeAssignableTo<IShipRepository>();
        }
    }
}
