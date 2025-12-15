using AEChallenge.Application.DTOs.Request;
using FluentAssertions;
using Xunit;

namespace AEChallenge.Application.Tests.DTOs
{
    public class DtoValidationTests
    {
        #region ShipCreateDto Tests

        [Fact]
        public void ShipCreateDto_WithValidData_ShouldPassValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeTrue();
            ErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void ShipCreateDto_WithEmptyShipCode_ShouldFailValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
            ErrorMessage.Should().Contain("Ship Code is required");
        }

        [Fact]
        public void ShipCreateDto_WithShortShipCode_ShouldFailValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "AB",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
            ErrorMessage.Should().Contain("between");
        }

        [Fact]
        public void ShipCreateDto_WithInvalidCharactersInShipCode_ShouldFailValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "SHIP@001",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
            ErrorMessage.Should().Contain("alphanumeric");
        }

        [Fact]
        public void ShipCreateDto_WithEmptyShipName_ShouldFailValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "",
                FiscalYear = 2025,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
            ErrorMessage.Should().Contain("Ship Name is required");
        }

        [Fact]
        public void ShipCreateDto_WithTooLongShipName_ShouldFailValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = new string('A', 151),
                FiscalYear = 2025,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
            ErrorMessage.Should().Contain("cannot exceed");
        }

        [Fact]
        public void ShipCreateDto_WithInvalidFiscalYear_ShouldFailValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 1999,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
            ErrorMessage.Should().Contain("Fiscal Year");
        }

        [Fact]
        public void ShipCreateDto_WithInvalidStatus_ShouldFailValidation()
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = "InvalidStatus"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
            ErrorMessage.Should().Contain("Invalid Status");
        }

        [Theory]
        [InlineData("Active")]
        [InlineData("Inactive")]
        [InlineData("active")]
        [InlineData("INACTIVE")]
        public void ShipCreateDto_WithValidStatus_ShouldPassValidation(string status)
        {

            var dto = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 2025,
                Status = status
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeTrue();
        }

        [Fact]
        public void ShipCreateDto_WithBoundaryFiscalYear_ShouldPassValidation()
        {

            var dtoMin = new ShipCreateDto
            {
                ShipCode = "SHIP001",
                ShipName = "Test Ship",
                FiscalYear = 2000,
                Status = "Active"
            };


            var dtoMax = new ShipCreateDto
            {
                ShipCode = "SHIP002",
                ShipName = "Test Ship 2",
                FiscalYear = 2100,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = dtoMin.Validate();
            var resultMax = dtoMax.Validate();


            IsValid.Should().BeTrue();
            resultMax.IsValid.Should().BeTrue();
        }

        #endregion

        #region ShipListParameters Tests

        [Fact]
        public void ShipListParameters_WithValidData_ShouldPassValidation()
        {

            var parameters = new ShipListParameters
            {
                PageNumber = 1,
                PageSize = 10,
                Status = "Active"
            };


            var (IsValid, ErrorMessage) = parameters.Validate();


            IsValid.Should().BeTrue();
        }

        [Fact]
        public void ShipListParameters_WithInvalidPageNumber_ShouldFailValidation()
        {

            var parameters = new ShipListParameters
            {
                PageNumber = 0,
                PageSize = 10
            };


            var (IsValid, ErrorMessage) = parameters.Validate();


            IsValid.Should().BeFalse();
        }

        #endregion

        #region ShipAssignmentDto Tests

        [Fact]
        public void ShipAssignmentDto_WithValidData_ShouldPassValidation()
        {

            var dto = new ShipAssignmentDto
            {
                UserId = 1,
                ShipCode = "SHIP001"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeTrue();
        }

        [Fact]
        public void ShipAssignmentDto_WithInvalidUserId_ShouldFailValidation()
        {

            var dto = new ShipAssignmentDto
            {
                UserId = 0,
                ShipCode = "SHIP001"
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
        }

        [Fact]
        public void ShipAssignmentDto_WithEmptyShipCode_ShouldFailValidation()
        {

            var dto = new ShipAssignmentDto
            {
                UserId = 1,
                ShipCode = ""
            };


            var (IsValid, ErrorMessage) = dto.Validate();


            IsValid.Should().BeFalse();
        }

        #endregion
    }
}
