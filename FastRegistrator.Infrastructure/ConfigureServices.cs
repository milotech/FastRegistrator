using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Persistence;
using FastRegistrator.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

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

            services.AddHttpClient<IICService, ICService>(options =>
            {
                var url = configuration[PrizmaServiceUrl];
                options.BaseAddress = new Uri(url);
            });

            services.AddSqlServer<ApplicationDbContext>(
                configuration.GetConnectionString("FastRegConnection"),
                opts => opts.EnableRetryOnFailure()
            );
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ApplicationDbContextInitialiser>();

            services.RegisterEventBus(configuration);

            services.AddSingleton<ICommandExecutor>(sp =>
            {
                var commandsAssembly = Assembly.GetAssembly(typeof(ICommandExecutor))!;
                var logger = sp.GetRequiredService<ILogger<CommandExecutor.CommandExecutor>>();
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var appLifeTime = sp.GetRequiredService<IHostApplicationLifetime>();

                return new CommandExecutor.CommandExecutor(scopeFactory, commandsAssembly, logger, appLifeTime.ApplicationStopping);
            });

            return services;
        }

    }
}
