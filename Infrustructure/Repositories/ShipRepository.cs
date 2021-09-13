using Domain.Entities;
using Domain.Interfaces;
using Infrustructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrustructure.Repositories
{

    public class ShipRepository : IShipRepository
    {
        private readonly AppDbContext _context;

        public ShipRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ship>> GetShipsAsync()
        {
            return await _context.Ships.ToListAsync();
        }

        public async Task<Ship> GetShipAsync(int id)
        {
            return await _context.Ships.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async void CreateShip(Ship ship)
        {
            _context.Ships.Add(ship);
            await _context.SaveChangesAsync(); 
        }

        public async void UpdateVelocity(int id, Ship ship)
        {
            var item = _context.Ships.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Velocity = ship.Velocity;
                _context.Ships.Update(item);
            }

            await _context.SaveChangesAsync();
        }
    }
}
