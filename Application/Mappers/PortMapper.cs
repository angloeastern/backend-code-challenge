using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappers
{
    public class PortMapper : IPortMapper
    {
        public List<PortDTO> Map(IEnumerable<Port> ports) => ports.Select(Map).ToList();

        public PortDTO Map(Port port) => new PortDTO()
        {
            Name = port.Name,
            Lat = port.Lat,
            Lon = port.Lon,
            Id = port.Id
        };
    }
}
