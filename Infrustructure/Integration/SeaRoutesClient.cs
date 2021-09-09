using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using RestSharp;
using RestSharp.Serialization.Json;

namespace Infrustructure.Integration
{
    public interface ISeaRoutesClient
    {
        Task<SeaRoutesResult> GetClosestPort(SeaRoutesRequest requestParams);
    }

    public class SeaRoutesClient : ISeaRoutesClient
    {
        private readonly IRestClient _restClient;

        public SeaRoutesClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<SeaRoutesResult> GetClosestPort(SeaRoutesRequest requestParams)
        {
            var req = new RestRequest($"route/lon:{requestParams.StartCoordLon}lat:{requestParams.StartCoordLat}/lon:{requestParams.EndCoordLon}lat:{requestParams.EndCoordLat}");
            req.AddQueryParameter("speed", requestParams.Velocity.ToString());
            return await _restClient.GetAsync<SeaRoutesResult>(req, CancellationToken.None);
        }
    }


}
