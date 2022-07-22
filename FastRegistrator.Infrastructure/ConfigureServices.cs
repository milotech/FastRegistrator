using FastRegistrator.Infrastructure.Interfaces;
using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.Infrastructure
{
    public static class ConfigureServices
    {

        private const string PrizmaServiceUrl = "PrizmaService:Url";

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IPrizmaService, PrizmaService>(options =>
            {
                var url = configuration.GetSection(PrizmaServiceUrl).Value;
                options.BaseAddress = new Uri(url);
            });
            services.RegisterEventBus(configuration);
            services.AddSingleton<IDateTime, DateTimeService>();

            return services;
        }

    }
}
