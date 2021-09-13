using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;

namespace Application.Interfaces
{
    public interface IPortService
    {
        Task<List<PortDTO>> GetPortsAsync();
    }
}
