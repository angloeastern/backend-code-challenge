using Domain.Entities;
using Domain.Interfaces;
using Infrustructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Infrustructure.Repositories
{

    public class ShipRepository : IShipRepository
    {
        private readonly AppDbContext _context;

        public ShipRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Ship> GetShips()
        {
            return _context.Ships;
        }

        public Ship GetShip(int id)
        {
            return _context.Ships.FirstOrDefault(x => x.Id == id);
        }

        public void CreateShip(Ship ship)
        {
            _context.Ships.Add(ship);
            _context.SaveChanges();
        }

        public void UpdateVelocity(int id, Ship ship)
        {
            var item = _context.Ships.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Velocity = ship.Velocity;
                _context.Ships.Update(item);
            }

            _context.SaveChanges();
        }
    }
}
