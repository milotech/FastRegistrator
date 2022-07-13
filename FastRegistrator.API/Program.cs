using FastRegistrator.ApplicationCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
    );

builder.Services.AddControllers();
builder.Services.AddAplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();
app.UseAuthorization();

app.MapControllers();

app.Run();
