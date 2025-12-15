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
 
    /// Unit tests for AuthRepository
    /// Note: Since the repository uses stored procedures, these tests focus on
    /// constructor and interface validation.
    /// </summary>
    public class AuthRepositoryTests
    {
        [Fact]
        public void AuthRepository_Constructor_ShouldInitializeCorrectly()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockLogger = new Mock<ILogger<AuthRepository>>();


            var repository = new AuthRepository(context, mockLogger.Object);


            repository.Should().NotBeNull();
            repository.Should().BeOfType<AuthRepository>();
        }

        [Fact]
        public void AuthRepository_ShouldImplementIAuthRepository()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockLogger = new Mock<ILogger<AuthRepository>>();


            var repository = new AuthRepository(context, mockLogger.Object);


            repository.Should().BeAssignableTo<IAuthRepository>();
        }
    }
}
