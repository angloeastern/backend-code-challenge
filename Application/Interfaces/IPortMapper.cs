using Application.Dto;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IPortMapper
    {
        PortDTO Map(Port port);
        List<PortDTO> Map(IEnumerable<Port> ports);
    }
}
