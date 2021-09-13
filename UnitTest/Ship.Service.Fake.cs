using Application.Dto;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    public class ShipServiceFake : IShipService
    {
        private readonly List<ShipDTO> _ships;
        private readonly Random _random;

        public ShipServiceFake()
        {
            _ships = new List<ShipDTO>()
                {
                    new ShipDTO() {Id = 256,Name = "Ship1",Velocity = 25,Lat = "-12.123456",Lon ="5.123456" },
                    new ShipDTO() {Id = 11145,Name = "Ship2",Velocity = 10,Lat = "33.123456",Lon ="28.123456" },
                    new ShipDTO() {Id = 6698,Name = "Ship3",Velocity = 19,Lat ="44.123456" ,Lon = "25.123456"},
                };
            _random = new Random();
        }

        public async Task<IEnumerable<ShipDTO>> GetShips()
        {
            return await Task.Run(() => _ships);
        }

        public async Task<ShipDTO> GetShip(int id)
        {
            return await Task.Run(() => _ships.FirstOrDefault(predicate: x => x.Id == id));
        }

        public void CreateShip(ShipDTO ship)
        {
            ship.Id = _random.Next(minValue: 0, maxValue: 255);
            _ships.Add(item: ship);
        }

        public void UpdateVelocity(int id, ShipDTO ship)
        {
            var item = _ships.FirstOrDefault(predicate: x => x.Id == id);
            if (item == null) return;
            item.Velocity = ship.Velocity;
            _ships.Remove(item: ship);
            _ships.Add(item: item);
        }
    }
}
