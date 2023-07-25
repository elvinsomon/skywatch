using System.ComponentModel.DataAnnotations;
using MetricsShared.Common;

namespace SkyWatch.VisualizationSystem.UI.Data;

public class Alert
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public MetricType MetricType { get; set; }
    public double Threshold { get; set; }
    public string? EmailAddress { get; set; }
}