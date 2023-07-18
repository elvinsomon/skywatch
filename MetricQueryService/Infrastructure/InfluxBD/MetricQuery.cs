using InfluxDB.Client;
using MetricQueryService.Domain.Contracts;
using MetricQueryService.Domain.Models;
using MetricsShared.Common;
using Microsoft.Extensions.Configuration;

namespace MetricQueryService.Infrastructure.InfluxBD;

public class MetricQuery: IMetricQuery
{
    private readonly InfluxConfiguration _influxConfiguration;

    public MetricQuery(IConfiguration configuration)
    {
        _influxConfiguration = new InfluxConfiguration();
        configuration.Bind("InfluxConfig", _influxConfiguration);
    }

    public async Task GetMetricsAsync(MetricType metricType)
    {
        var metricName = Enum.GetName(typeof(MetricType), metricType);
                    
        var flux = $"from(bucket: \"{_influxConfiguration.Bucket}\") " +
                   $"|> range(start: -1h) " +
                   $"|> filter(fn: (r) => r[\"_measurement\"] == \"Infra_Perform\") " +
                   $"|> filter(fn: (r) => r[\"_field\"] == \"{metricName}\") " +
                   $"|> aggregateWindow(every: 1m, fn: mean, createEmpty: false) " +
                   $"|> yield(name: \"mean\")";

        using var client = new InfluxDBClient(_influxConfiguration.Host, _influxConfiguration.Token);

        while (true)
        {
            var fluxTables = await client.GetQueryApi().QueryAsync(flux, _influxConfiguration.Organization);

            fluxTables.ForEach(fluxTable =>
            {
                fluxTable.Records.ForEach(fluxRecord =>
                {
                    Console.WriteLine($"{fluxRecord.GetTime()}: {fluxRecord.GetValue()}");
                });
            });
            
            await Task.Delay(2000);
        }
    }
}

public class InfluxConfiguration
{
    public string? Host { get; set; }
    public string? Token { get; set; }
    public string? Bucket { get; set; }
    public string? Organization { get; set; }
}