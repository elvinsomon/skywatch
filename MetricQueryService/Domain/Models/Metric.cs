namespace MetricQueryService.Domain.Models;

public class Metric
{
    public string? Id { get; set; }
    public string? Hostname { get; set; }
    public string? IpAddress { get; set; }
    public string? MetricName { get; set; }
    public string? MetricValue { get; set; }
    public DateTime TimesTamp { get; set; }
    public string? TimesTampString { get; set; }
}