namespace MetricsShared.MetricsCollectorDTO;

public record MCRequest
{
    public string? Hostname { get; set; }
    public string? IpAddress { get; set; }
    public string? MetricName { get; set; }
    public string? MetricValue { get; set; }
    public string? TimesTamp { get; set; }
}