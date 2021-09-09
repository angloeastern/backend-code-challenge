using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Mappers
{
    public interface IShipMapper
    {
        ShipDTO Map(Ship ship);
        List<ShipDTO> Map(IEnumerable<Ship> ships);
        Ship Map(ShipDTO ship) => new Ship();
    }

    public class ShipMapper : IShipMapper
    {
        public ShipMapper()
        {

        }

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
