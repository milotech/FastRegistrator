using FastRegistrator.ApplicationCore;
using FastRegistrator.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting the FastRegistrator Service...");

try
{ 
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
            .ReadFrom.Configuration(context.Configuration)
            .WriteTo.Console()
        );

    builder.Services.AddControllers();
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddSwaggerGen(x => {
        x.CustomSchemaIds(i => i.FullName);
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline. 

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();
    app.UseAuthorization();

    app.MapControllers();

    app.UseEventBus();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("FastRegistrator stopped");
    Log.CloseAndFlush();
}