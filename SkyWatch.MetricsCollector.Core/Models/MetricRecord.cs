namespace SkyWatch.MetricsCollector.Core.Models;

public class MetricRecord
{
    public string? Id { get; set; }
    public string? Hostname { get; set; }
    public string? IpAddress { get; set; }
    public string? MetricName { get; set; }
    public string? MetricValue { get; set; }
    public DateTime TimesTamp { get; set; }
}