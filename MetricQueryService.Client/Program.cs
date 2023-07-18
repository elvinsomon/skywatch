// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using MetricQueryService;
using MetricQueryService.Infrastructure.InfluxBD;
using MetricsShared.Common;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var metricQuery = new MetricQuery(configuration);
var service = new MetricsQueryService(metricQuery);

await service.GetMetricsAsync(MetricType.cpu_usage);

Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);