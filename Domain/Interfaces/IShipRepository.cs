using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IShipRepository
    {
        IEnumerable<Ship> GetShips();
        Ship GetShip(int id);
        void CreateShip(Ship ship);
        void UpdateVelocity(int id, Ship ship);
    }
}
