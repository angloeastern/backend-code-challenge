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
    public class UserRepositoryTests
    {
        [Fact]
        public void UserRepository_Constructor_ShouldInitializeCorrectly()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockCurrentUserService = new Mock<ICurrentUserService>();
            mockCurrentUserService.Setup(x => x.UserId).Returns("1");
            mockCurrentUserService.Setup(x => x.UserName).Returns("TestUser");
            var mockLogger = new Mock<ILogger<UserRepository>>();


            var repository = new UserRepository(context, mockCurrentUserService.Object, mockLogger.Object);


            repository.Should().NotBeNull();
            repository.Should().BeOfType<UserRepository>();
        }

        [Fact]
        public void UserRepository_ShouldImplementIUserRepository()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockLogger = new Mock<ILogger<UserRepository>>();


            var repository = new UserRepository(context, mockCurrentUserService.Object, mockLogger.Object);


            repository.Should().BeAssignableTo<IUserRepository>();
        }
    }
}
