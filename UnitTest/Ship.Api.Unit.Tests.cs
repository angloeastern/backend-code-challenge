using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Models;
using Infrustructure.Integration;
using Infrustructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class ShipTest
    {
        private IShipService _shipService;
        private ISeaRoutesClient _client;

        public ShipTest()
        {
            _shipService = new ShipServiceFake();
            _client = new SeaRoutesClientFake(restClient: new RestClient());
        }

        [Fact]
        public void ShouldReurnShips()
        {
            var ships = _shipService.GetShips();
            Assert.NotNull(ships);
        }

        [Fact]
        public void SouldReturnShip()
        {
            var ship =  _shipService.GetShip(256);
            Assert.NotNull(ship);  
        }

        [Fact]
        public async void ShouldReturnNull()
        {
            var ship = await _shipService.GetShip(111);
            Assert.Null(ship);
        }
        
        [Fact]
        public async void AreEqual()
        {
            var newShip = new ShipDTO() { Id = 256, Name = "Ship1", Velocity = 25, Lat = "-12.123456", Lon = "5.123456" };
            var ship = await _shipService.GetShip(256);
            Assert.Equal(ship,newShip);
        }

        [Fact]
        public void CreatedShip()
        {
            var newShip = new ShipDTO() { Id = 125, Name = "Ship65", Velocity = 22, Lat = "41.123456", Lon = "22.123456" };

            _shipService.CreateShip(newShip);
            var result = _shipService.GetShip(125);

            Assert.IsType<ShipDTO>(result);
            Assert.Equal(result.Result, newShip);
        }

        [Fact]
        public void CannotCreateWithoutRequiredField()
        {
            var newShip = new ShipDTO() { Name = "Ship65", Velocity = 22, Lat = "41.123456", Lon = "22.123456" };

            Assert.Throws<MissingMemberException>(()=> _shipService.CreateShip(newShip));
        }

        [Fact]
        public void UpdatedShip()
        {
            var newShip = new ShipDTO() { Id=125, Name = "Ship65", Velocity = 12, Lat = "41.123456", Lon = "22.123456" };

            var updShipResult = _shipService.GetShip(125);
            _shipService.UpdateVelocity(125,newShip);

            Assert.Equal<ShipDTO>(newShip, updShipResult.Result);
        }

        [Fact]
        public void GetClosestPort()
        {
            var okResult = _client.GetClosestPortAsync(SeaRoutesClientFake.GetRequest());
            Assert.IsType<Task<SeaRoutesResult>>(okResult);
        }

    }
}
