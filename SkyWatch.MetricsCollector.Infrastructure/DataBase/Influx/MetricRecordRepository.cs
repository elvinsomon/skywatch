using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Configuration;
using SkyWatch.MetricsCollector.Core.Contracts;
using SkyWatch.MetricsCollector.Core.Models;

namespace SkyWatch.MetricsCollector.Infrastructure.DataBase.Influx;

public class MetricRecordRepository : IMetricRecordRepository
{
    // private const string Token = "faZs9WpkT5PSm_hy1iym8OmqoQwvoFr7fs96RSYR_jENOFvDgxhT9_WnK1R7jIaJLuqWp5q15fFJkUrKhc3eyg==";
    // private const string Bucket = "MetricsCollector";
    // private const string Org = "SkyWatch";
    private readonly DataBaseConfig _dataBaseConfig;

    public MetricRecordRepository(IConfiguration configuration)
    {
        _dataBaseConfig = new DataBaseConfig();
        configuration.Bind("InfluxConfig", _dataBaseConfig);
    }
    
    public async Task CreateMetricRecordAsync(MetricRecord metricRecord)
    {
        using var influxDbClient = new InfluxDBClient(_dataBaseConfig.Host, _dataBaseConfig.Token);

        var point = PointData
            .Measurement("Infra_Perform")
            .Tag("host", metricRecord.Hostname)
            .Tag("ip_address", metricRecord.IpAddress)
            .Field(metricRecord.MetricName, double.Parse(metricRecord.MetricValue)) // Varificar el datatype de MetricValue correcto
            .Timestamp(metricRecord.TimesTamp, WritePrecision.Ns);

        using var writeApi = influxDbClient.GetWriteApi();
        writeApi.WritePoint(point, _dataBaseConfig.Bucket, _dataBaseConfig.Organization);

        await Task.CompletedTask;
    }

    private record DataBaseConfig
    {
        public string? Host { get; set; }
        public string? Token { get; set; }
        public string? Bucket { get; set; }
        public string? Organization { get; set; }
    }
}