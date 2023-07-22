using MetricQueryService.Domain.Models;
using MetricsShared.Common;

namespace MetricQueryService.Domain.Contracts;

public interface IMetricQuery
{
    Task<List<Metric>> GetMetricsAsync(MetricType metricType);
}