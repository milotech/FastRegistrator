using FastRegistrator.ApplicationCore.IntegrationEvents.Events;
using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.EventBus;
using FastRegistrator.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Infrastructure
{
    internal static class RabbitMqConstants
    {
        public static class Exchanges
        {
            public const string FastRegistrator = "FastReg";
            public const string MobileApp = "MobileApp";
            public const string ClientsService = "Clients";
            public const string Service1C = "1C-Service";
        }

        public static class RoutingKeys
        {

        }
    }

    internal static class ConfigureEventBus
    {
        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventBusConnectionSettings>(configuration.GetSection("EventBus"));
            services.AddSingleton<RabbitMqConnection>();
            services.AddSingleton<IEventBus, RabbitMqEventBus>((sp) =>
            {
                var connection = sp.GetRequiredService<RabbitMqConnection>();
                var logger = sp.GetRequiredService<ILogger<RabbitMqEventBus>>();
                var appLifeTyime = sp.GetRequiredService<IHostApplicationLifetime>();

                var eventBus = new RabbitMqEventBus(connection, logger, sp, appLifeTyime.ApplicationStopping);
                eventBus.ConfigureRabbitMqEvents();
                return eventBus;
            });
        }

        private static void ConfigureRabbitMqEvents(this RabbitMqEventBus rabbitMq)
        {
            // incoming messages configuration
            rabbitMq.ConfigureEvent<TestIntegrationEvent>(RabbitMqConstants.Exchanges.FastRegistrator);
            rabbitMq.ConfigureEvent<PersonCheckRequestedEvent>(RabbitMqConstants.Exchanges.FastRegistrator);
            rabbitMq.ConfigureEvent<ESIANotApprovedEvent>(RabbitMqConstants.Exchanges.FastRegistrator);

            // outcoming messages configuration
            // ...
        }
    }
}
