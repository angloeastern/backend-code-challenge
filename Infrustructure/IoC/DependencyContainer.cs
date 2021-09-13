using Application.Interfaces;
using Application.Services;
using Infrustructure.Integration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace Infrustructure.IoC
{
    public static class DependencyContainer
    {

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IShipService, ShipService>();
            services.AddTransient<IPortService, PortService>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<ISeaRoutesClient, SeaRoutesClient>();
        }
    }
}
