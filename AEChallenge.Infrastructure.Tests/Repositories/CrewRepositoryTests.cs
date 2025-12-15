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
 
    public class CrewRepositoryTests
    {
        [Fact]
        public void CrewRepository_Constructor_ShouldInitializeCorrectly()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockLogger = new Mock<ILogger<CrewRepository>>();


            var repository = new CrewRepository(context, mockLogger.Object);


            repository.Should().NotBeNull();
            repository.Should().BeOfType<CrewRepository>();
        }

        [Fact]
        public void CrewRepository_ShouldImplementICrewRepository()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockLogger = new Mock<ILogger<CrewRepository>>();


            var repository = new CrewRepository(context, mockLogger.Object);


            repository.Should().BeAssignableTo<ICrewRepository>();
        }
    }
}
