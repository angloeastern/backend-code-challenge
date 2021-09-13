using Application.Dto;
using Domain.Entities;
using System.Collections.Generic;


namespace Application.Interfaces
{
    public interface IShipMapper
    {
        ShipDTO Map(Ship ship);
        List<ShipDTO> Map(IEnumerable<Ship> ships);
        Ship Map(ShipDTO ship) => new Ship();
    }
}
