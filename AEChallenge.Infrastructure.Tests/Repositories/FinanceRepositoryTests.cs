using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Data;
using AEChallenge.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AEChallenge.Infrastructure.Tests.Repositories
{
    public class FinanceRepositoryTests
    {
        [Fact]
        public void FinanceRepository_Constructor_ShouldInitializeCorrectly()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockLogger = new Mock<ILogger<FinanceRepository>>();


            var repository = new FinanceRepository(context, mockLogger.Object);


            repository.Should().NotBeNull();
            repository.Should().BeOfType<FinanceRepository>();
        }

        [Fact]
        public void FinanceRepository_ShouldImplementIFinanceRepository()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockLogger = new Mock<ILogger<FinanceRepository>>();


            var repository = new FinanceRepository(context, mockLogger.Object);


            repository.Should().BeAssignableTo<IFinanceRepository>();
        }
    }
}
