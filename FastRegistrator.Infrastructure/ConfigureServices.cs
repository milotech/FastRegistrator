using FastRegistrator.Infrastructure.EventBus;
using FastRegistrator.Infrastructure.Options;
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
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventBusConnectionSettings>(configuration.GetSection("EventBus"));
            services.AddSingleton<RabbitMqConnection>();
            services.AddSingleton<RabbitMqEventBus>();

            return services;
        }

    }
}
