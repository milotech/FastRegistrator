using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
