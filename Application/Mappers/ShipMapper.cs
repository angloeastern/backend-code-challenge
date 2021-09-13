using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappers
{
    public class ShipMapper : IShipMapper
    {
        public List<ShipDTO> Map(IEnumerable<Ship> ships) => ships.Select(Map).ToList();

        public ShipDTO Map(Ship ship) => new ShipDTO()
        {
            Name = ship.Name,
            Lat = ship.Lat,
            Lon = ship.Lon,
            Velocity = ship.Velocity
        };

        public Ship Map(ShipDTO ship) => new Ship()
        {
            Name = ship.Name,
            Lat = ship.Lat,
            Lon = ship.Lon,
            Velocity = ship.Velocity
        };
    }
}
