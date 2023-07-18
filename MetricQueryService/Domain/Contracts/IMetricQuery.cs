using MetricQueryService.Domain.Models;
using MetricsShared.Common;

namespace MetricQueryService.Domain.Contracts;

public interface IMetricQuery
{
    Task<IEnumerable<Metric>> GetMetricsAsync(MetricType metricType);
}