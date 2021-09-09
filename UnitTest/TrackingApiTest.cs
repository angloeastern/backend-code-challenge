using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Infrustructure.Integration;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using ShipTrackingAPI.Controllers;
using Xunit;

namespace UnitTest
{
    public class TrackingApiTest
    {
        private TrackingController _controller;
        private IShipService _shipService;
        private IPortService _portService;
        private ISeaRoutesClient _client;
        private readonly Random _random;

        public TrackingApiTest()
        {
            _shipService = new ShipServiceFake();
            _portService = new PortServiceFake();
            _client = new SeaRoutesClientFake(restClient: new RestClient());
            _controller = new TrackingController(portService: _portService, shipService: _shipService, client: _client);
        }

        #region GetShips

        [Fact]
        public void GetShips_WhenCalled_ReturnsOkResult()
        {
            var okResult = _controller.GetShips();

            Assert.IsType<OkObjectResult>(@object: okResult.Result);
        }

        [Fact]
        public void GetShips_WhenCalled_ReturnsAllItems()
        {
            var okResult = _controller.GetShips().Result as OkObjectResult;

            var items = Assert.IsType<List<ShipDTO>>(@object: okResult.Value);

            Assert.Equal(expected: 3, actual: items.Count);
        }

        #endregion

        #region GetShip

        [Fact]
        public void GetShip_UnknownIddPassed_ReturnsNotFoundResult()
        {
            var notFoundResult = _controller.GetShip(id: _random.Next(minValue: 300, maxValue: 500));

            Assert.IsType<NotFoundResult>(@object: notFoundResult.Result);
        }

        [Fact]
        public void GetShip_ExistingIdPassed_ReturnsOkResult()
        {
            var items = _shipService.GetShips().FirstOrDefault();
            var okResult = _controller.GetShip(id: items.Id);

            Assert.IsType<OkObjectResult>(@object: okResult.Result);
        }
        [Fact]
        public void GetShip_ExistingGuidPassed_ReturnsRightItem()
        {
            var items = _shipService.GetShips().FirstOrDefault();
            var okResult = _controller.GetShip(id: items.Id).Result as OkObjectResult;

            Assert.IsType<ShipDTO>(@object: okResult.Value);
            Assert.Equal(expected: items.Id, actual: ((ShipDTO)okResult.Value).Id);
        }

        #endregion

        #region CrateShip

        [Fact]
        public void CreateShip_InvalidObjectPassed_ReturnsBadRequest()
        {
            var nameMissingItem = new ShipDTO()
            {
                Name = "NewShip",
                Lat = "13.123456",
                Lon = "24,123456"
            };

            _controller.ModelState.AddModelError("Id", "Required");

            var badResponse = _controller.CreateShip(nameMissingItem);

            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void CreateShip_ValidObjectPassed_ReturnsCreatedResponse()
        {
            var testItem = new ShipDTO()
            {
                Id = _random.Next(500, 1000),
                Name = "NewShip1",
                Velocity = 11,
                Lat = "45.123456",
                Lon = "54.123456"
            };

            var createdResponse = _controller.CreateShip(testItem);

            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }
        [Fact]
        public void CreateShip_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            var testItem = new ShipDTO()
            {
                Id = _random.Next(1000, 1050),
                Name = "NewShip2",
                Velocity = 13,
                Lat = "25.123456",
                Lon = "24.123456"
            };

            var createdResponse = _controller.CreateShip(testItem) as CreatedAtActionResult;
            var item = createdResponse.Value as ShipDTO;

            Assert.IsType<ShipDTO>(item);
            Assert.Equal("NewShip2", item.Name);
        }
        #endregion

        #region UpdateVelocity

        [Fact]
        public void UpdateVelocity_ValidObjectPassed_ReturnsCreatedResponse()
        {
            var item = _shipService.GetShips().FirstOrDefault();
            item.Velocity = 45;
            var createdResponse = _controller.UpdateVelocity(item.Id, item);

            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        #endregion

        #region GetClosestPort

        [Fact]
        public void GetClosestPort()
        {
            var okResult = _client.GetClosestPort(SeaRoutesClientFake.GetRequest());
            Assert.IsType<OkObjectResult>(@object: okResult.Result);
        }

        #endregion
    }
}