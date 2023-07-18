using MetricQueryService.Domain.Contracts;
using MetricsShared.Common;

namespace MetricQueryService;

public class MetricsQueryService
{
    private readonly IMetricQuery _metricQuery;

    public MetricsQueryService(IMetricQuery metricQuery)
    {
        _metricQuery = metricQuery;
    }
    
    public async Task GetMetricsAsync(MetricType metricType)
    {
        await _metricQuery.GetMetricsAsync(metricType);
    }
}