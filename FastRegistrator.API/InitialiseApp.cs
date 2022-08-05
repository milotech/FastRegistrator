using FastRegistrator.ApplicationCore;
using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Persistence;

namespace FastRegistrator.API
{
    public static class InitialiseApp
    {
        public static async Task InitialiseAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
            }

            var eventBus = app.ApplicationServices.GetService<IEventBus>();
            if(eventBus != null)
                eventBus.StartApplicationSubscriptions();
        }
    }
}
