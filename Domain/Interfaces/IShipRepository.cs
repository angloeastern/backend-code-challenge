using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IShipRepository
    {
        Task<IEnumerable<Ship>> GetShipsAsync();
        Task<Ship> GetShipAsync(int id);
        void CreateShip(Ship ship);
        void UpdateVelocity(int id, Ship ship);
    }
}
