using System.Collections.Generic;
using Application.Dto;

namespace Application.Interfaces
{
    public interface IPortService
    {
        List<PortDTO> GetPorts();
    }
}
