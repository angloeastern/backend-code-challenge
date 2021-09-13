using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPortRepository
    {
        Task<IEnumerable<Port>> GetPortsAsync();
    }
}
