using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IShipService
    {
        Task<IEnumerable<ShipDTO>> GetShips();
        Task<ShipDTO> GetShip(int id);
        void CreateShip(ShipDTO ship);
        void UpdateVelocity(int id, ShipDTO ship);
    }
}
