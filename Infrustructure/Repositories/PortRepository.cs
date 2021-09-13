using Domain.Entities;
using Domain.Interfaces;
using Infrustructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrustructure.Repositories
{
    public class PortRepository : IPortRepository
    {
        public readonly AppDbContext _context;

        public PortRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Port>> GetPortsAsync()
        {
            return await _context.Ports.ToListAsync();
        }
    }
}
