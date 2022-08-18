using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.ApplicationCore.Startup;
using FastRegistrator.Infrastructure.Persistence;

namespace FastRegistrator.API
{
    public static class InitialiseApp
    {
        public static async Task InitialiseAsync(this IApplicationBuilder app)
        {
            // Database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
            }

            // EventBus
            var eventBus = app.ApplicationServices.GetService<IEventBus>();
            if (eventBus != null)
            {
                eventBus.StartApplicationSubscriptions();
            }

            // Incompleted registrations
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var registrationsRecoverer = scope.ServiceProvider.GetRequiredService<RegistrationsRecoverer>();
                var appHostLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                await registrationsRecoverer.RecoverIncompletedRegistrations(appHostLifetime.ApplicationStopping);
            }
        }
    }
}
