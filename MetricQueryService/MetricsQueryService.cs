using MetricQueryService.Domain.Contracts;
using MetricQueryService.Domain.Models;
using MetricsShared.Common;

namespace MetricQueryService;

public class MetricsQueryService
{
    private readonly IMetricQuery _metricQuery;

    public MetricsQueryService(IMetricQuery metricQuery)
    {
        _metricQuery = metricQuery;
    }
    
    public async Task<List<Metric>> GetMetricsAsync(MetricType metricType)
        => await _metricQuery.GetMetricsAsync(metricType);
}