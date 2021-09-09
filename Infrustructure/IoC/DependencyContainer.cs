using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Infrustructure.Integration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace Infrustructure.IoC
{
    public class DependencyContainer
    {

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IShipService, ShipService>();
            services.AddScoped<IPortService, PortService>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<ISeaRoutesClient, SeaRoutesClient>();
        }
    }
}
