using Domain.Models;
using Infrustructure.Integration;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest
{
    public class SeaRoutesClientFake : ISeaRoutesClient
    {
        private static SeaRoutesRequest _request;
        private readonly IRestClient _restClient;
        public SeaRoutesClientFake(IRestClient restClient)
        {
            _request = new SeaRoutesRequest()
            {
                StartCoordLat = "3.123456",
                StartCoordLon = "5.123456",
                Velocity = 8,
                EndCoordLat = "13.123456",
                EndCoordLon = "21.123456"
            };

            _restClient = restClient;
        }


        public Task<SeaRoutesResult> GetClosestPortAsync(SeaRoutesRequest requestParams, CancellationToken cancellationToken = default)
        {
            var req = new RestRequest(resource: $"route/lon:{requestParams.StartCoordLon}lat:{requestParams.StartCoordLat}/lon:{requestParams.EndCoordLon}lat:{requestParams.EndCoordLat}");
            req.AddQueryParameter(name: "speed", value: requestParams.Velocity.ToString());
            return _restClient.GetAsync<SeaRoutesResult>(request: req, cancellationToken);
        }

        public static SeaRoutesRequest GetRequest() => _request;

    }
}
