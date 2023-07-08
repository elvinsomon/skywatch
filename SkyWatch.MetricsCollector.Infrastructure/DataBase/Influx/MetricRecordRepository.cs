using SkyWatch.MetricsCollector.Core.Contracts;
using SkyWatch.MetricsCollector.Core.Models;

namespace SkyWatch.MetricsCollector.Infrastructure.DataBase.Influx;

public class MetricRecordRepository : IMetricRecordRepository
{
    public Task CreateMetricRecordAsync(MetricRecord metricRecord)
    {
        throw new NotImplementedException();
    }
}