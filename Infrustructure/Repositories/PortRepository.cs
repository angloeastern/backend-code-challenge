using Domain.Entities;
using Domain.Interfaces;
using Infrustructure.Persistence;
using System.Collections.Generic;

namespace Infrustructure.Repositories
{
    public class PortRepository : IPortRepository
    {
        public AppDbContext _context;

        public PortRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Port> GetPorts()
        {
            return _context.Ports;
        }
    }
}
