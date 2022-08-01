using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Persistence;
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
            services.AddSingleton<IDateTime, DateTimeService>();
            services.AddHttpClient<IPrizmaService, PrizmaService>(options =>
            {
                var url = configuration[PrizmaServiceUrl];
                options.BaseAddress = new Uri(url);
            });
            services.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString("FastRegConnection"));
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ApplicationDbContextInitialiser>();
            services.RegisterEventBus(configuration);            

            return services;
        }

    }
}
