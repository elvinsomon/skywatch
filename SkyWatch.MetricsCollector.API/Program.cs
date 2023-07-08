using Serilog;
using SkyWatch.MetricsCollector.Core;
using SkyWatch.MetricsCollector.Core.Contracts;
using SkyWatch.MetricsCollector.Infrastructure.DataBase.Mock;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Information("Sky Watch. Metrics Collector API. Starting up");

try
{
    builder.Services.AddTransient<IMetricRecordRepository, MetricRecordRepositoryMock>();
    builder.Services.AddTransient<MetricCollectorService>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Sky Watch. Metrics Collector API. Unhandled exception");
}
finally
{
    Log.Information("Sky Watch. Metrics Collector API. Shut down complete");
    Log.CloseAndFlush();
}