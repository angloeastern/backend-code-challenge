using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Port> Ports { get; set; }
        public DbSet<Ship> Ships { get; set; }

    }
}
