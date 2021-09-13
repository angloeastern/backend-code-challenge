using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Infrustructure.Integration
{
    public interface ISeaRoutesClient
    {
        Task<SeaRoutesResult> GetClosestPortAsync(SeaRoutesRequest requestParams, CancellationToken cancellationToken = default);
    }
}
