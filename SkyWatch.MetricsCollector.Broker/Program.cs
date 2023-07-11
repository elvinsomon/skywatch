using Hangfire;
using Hangfire.SqlServer;
using Serilog;
using SkyWatch.MetricsCollector.Core;
using SkyWatch.MetricsCollector.Core.Contracts;
using SkyWatch.MetricsCollector.Infrastructure.DataBase.Influx;
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

Log.Information("Sky Watch. Metrics Collector Broker. Starting up");

try
{
    // Bussiness services
    builder.Services.AddTransient<IMetricRecordRepository, MetricRecordRepository>();
    // builder.Services.AddTransient<IMetricRecordRepository, MetricRecordRepositoryMock>();
    builder.Services.AddTransient<MetricCollectorService>();
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireServerDataBase"),
            new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

    builder.Services.AddHangfireServer(options =>
    {
        options.ServerName = $"SkyWatch.MetricsCollector.Broker:{Environment.MachineName}";
        options.WorkerCount = Environment.ProcessorCount * 5;
        options.HeartbeatInterval = TimeSpan.FromMinutes(5);
        options.Queues = new[] { "default" };
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseRouting();
    
    app.MapHangfireDashboard("/hangfire", new DashboardOptions
    {
        DashboardTitle = "Sky Watch Metrics Collector Broker - HangFire Server",
        AppPath = "/hangfire"
    });
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Sky Watch. Metrics Collector Broker. Unhandled exception");
}
finally
{
    Log.Information("Sky Watch. Metrics Collector Broker. Shut down complete");
    Log.CloseAndFlush();
}