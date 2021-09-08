using System.Collections.Generic;
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IShipService
    {
        IEnumerable<ShipDTO> GetShips();
        ShipDTO GetShip(int id);
        void CreateShip(ShipDTO ship);
        void UpdateVelocity(int id, ShipDTO ship);
    }
}
