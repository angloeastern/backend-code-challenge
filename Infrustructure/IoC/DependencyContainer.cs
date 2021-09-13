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
            services.AddScoped<IShipService, ShipService>();
            services.AddScoped<IPortService, PortService>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<ISeaRoutesClient, SeaRoutesClient>();
        }
    }
}
