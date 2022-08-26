using FastRegistrator.API;
using FastRegistrator.ApplicationCore;
using FastRegistrator.Infrastructure;
using Hellang.Middleware.ProblemDetails;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting the FastRegistrator Service...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
            .ReadFrom.Configuration(context.Configuration)
            .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}")
        );

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddConfiguredProblemDetails();
    builder.Services.AddSwaggerGen(x =>
    {
        x.CustomSchemaIds(i => i.FullName);
    });

    builder.Services.Configure<RouteOptions>(options =>
    {
        options.LowercaseUrls = true;
    });    

    var app = builder.Build();
    
    // Configure the HTTP request pipeline. 

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseAuthorization();
    app.UseProblemDetails();

    app.MapControllers();

    // Run application

    await app.InitialiseAsync();
    await app.RunAsync();
}
catch (Exception ex)
{
    #region ignore ef migrations StopTheHostException

    /* EF migrations tool throws StopTheHostException to stop the service, don't log the exception.
     * For .Net 7 the exception renamed to HostAbortedException.     * 
     * https://stackoverflow.com/questions/70247187/microsoft-extensions-hosting-hostfactoryresolverhostinglistenerstopthehostexce
    */
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException") || type.Equals("HostAbortedException"))
    {
        throw;
    }

    #endregion

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("FastRegistrator stopped");
    Log.CloseAndFlush();
}