using AEChallenge.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AEChallenge.Domain.Tests.Entities
{
    public class ShipTests
    {
        [Fact]
        public void Ship_Creation_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var ship = new Ship
            {
                ShipCode = "SHIP001",
                ShipName = "Flying Dutchman",
                FiscalYear = 112,
                Status = "Active",
                CreatedBy = "TestUser",
                CreatedDate = DateTime.UtcNow
            };

            // Assert
            ship.ShipCode.Should().Be("SHIP001");
            ship.ShipName.Should().Be("Flying Dutchman");
            ship.FiscalYear.Should().Be(112);
            ship.Status.Should().Be("Active");
            ship.CreatedBy.Should().Be("TestUser");
            ship.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Ship_WithActiveStatus_ShouldBeActive()
        {
            // Arrange
            var ship = new Ship
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 112,
                Status = "Active",
                CreatedBy = "TestUser",
                CreatedDate = DateTime.UtcNow
            };

            // Assert
            ship.Status.Should().Be("Active");
        }

        [Fact]
        public void Ship_WithInactiveStatus_ShouldBeInactive()
        {
            // Arrange
            var ship = new Ship
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 112,
                Status = "Inactive",
                CreatedBy = "TestUser",
                CreatedDate = DateTime.UtcNow
            };

            // Assert
            ship.Status.Should().Be("Inactive");
        }

        [Theory]
        [InlineData(112)] // January to December
        [InlineData(403)] // April to March
        [InlineData(701)] // July to June
        public void Ship_WithVariousFiscalYears_ShouldAcceptValidFormats(int fiscalYear)
        {
            // Arrange & Act
            var ship = new Ship
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = fiscalYear,
                Status = "Active",
                CreatedBy = "TestUser",
                CreatedDate = DateTime.UtcNow
            };

            // Assert
            ship.FiscalYear.Should().Be(fiscalYear);
        }
    }
}
