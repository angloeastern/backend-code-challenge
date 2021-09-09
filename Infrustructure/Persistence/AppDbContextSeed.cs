using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrustructure.Persistence
{
    public static class AppDbContextSeed
    {
        public static async Task SeedSampleDataAsync(AppDbContext context)
        {
            if (!context.Ships.Any())
            {
                context.Ships.AddRange(new Ship() { Lat = "38.45704", Lon = "27.15267", Name = "COSCO IZMIR", Velocity = 0 },
                                                   new Ship() { Lat = "42.63322", Lon = "18.12329", Name = "ECLIPSE", Velocity = 25 },
                                                   new Ship() { Lat = "50.47939", Lon = "-5.645242", Name = "BRITANNIA", Velocity = 12 });


            }

            if (!context.Ports.Any())
            {
                context.Ports.AddRange(new Port() { Name = "IZMIR", Lat = "38.45704", Lon = "27.15267" },
                                                   new Port() { Name = "SIRI", Lat = "37.742825", Lon = "24.068645" },
                                                   new Port() { Name = "LAMMA ISLAND", Lat = "22.21824", Lon = "114.10835" });
            }

            await context.SaveChangesAsync();
        }
    }
}
