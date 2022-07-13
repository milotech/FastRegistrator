using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FastRegistrator.ApplicationCore
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
