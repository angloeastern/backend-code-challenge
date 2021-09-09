using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SeaRoutesRequest
    {
        public string StartCoordLat { get; set; }
        public string StartCoordLon { get; set; }
        public string EndCoordLat { get; set; }
        public string EndCoordLon { get; set; }
        public decimal Velocity { get; set; }
    }
}
