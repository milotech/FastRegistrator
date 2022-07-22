using FastRegistrator.ApplicationCore;
using FastRegistrator.Infrastructure;
using Serilog;

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

app.Run();
