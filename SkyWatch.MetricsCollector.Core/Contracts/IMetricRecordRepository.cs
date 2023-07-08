using SkyWatch.MetricsCollector.Core.Models;

namespace SkyWatch.MetricsCollector.Core.Contracts;

public interface IMetricRecordRepository
{
    Task CreateMetricRecordAsync(MetricRecord metricRecord);
}