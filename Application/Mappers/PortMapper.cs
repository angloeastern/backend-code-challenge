using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Mappers
{
    public interface IPortMapper
    {
        PortDTO Map(Port port);
        List<PortDTO> Map(IEnumerable<Port> ports);
    }

    public class PortMapper : IPortMapper
    {
        public PortMapper()
        {

        }

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
