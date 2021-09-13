using Domain.Models;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace Infrustructure.Integration
{
    public class SeaRoutesClient : ISeaRoutesClient
    {
        private readonly IRestClient _restClient;

        public SeaRoutesClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<SeaRoutesResult> GetClosestPortAsync(SeaRoutesRequest requestParams, CancellationToken cancellationToken= default)
        {
            var req = new RestRequest($"route/lon:{requestParams.StartCoordLon}lat:{requestParams.StartCoordLat}/lon:{requestParams.EndCoordLon}lat:{requestParams.EndCoordLat}");
            req.AddQueryParameter("speed", requestParams.Velocity.ToString());
            return await _restClient.GetAsync<SeaRoutesResult>(req, cancellationToken);
        }
    }


}
