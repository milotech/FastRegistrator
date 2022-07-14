using FastRegistrator.Infrastructure;

using FastRegistrator.ApplicationCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
    );

builder.Services.AddControllers();
builder.Services.AddApplicationServices();

builder.Services.AddSwaggerGen(x => {
    x.CustomSchemaIds(i => i.FullName);
});
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline. 

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();
app.UseAuthorization();

app.MapControllers();

app.Run();
