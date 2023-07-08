using SkyWatch.MetricsCollector.Core.Contracts;
using SkyWatch.MetricsCollector.Core.Models;

namespace SkyWatch.MetricsCollector.Infrastructure.DataBase.Mock;

public class MetricRecordRepositoryMock : IMetricRecordRepository
{
    public async Task CreateMetricRecordAsync(MetricRecord metricRecord)
    {
        await Task.CompletedTask;
    }
}