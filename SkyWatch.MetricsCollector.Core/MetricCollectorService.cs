using MetricsShared.MetricsCollectorDTO;
using Microsoft.Extensions.Logging;
using SkyWatch.MetricsCollector.Core.Contracts;
using SkyWatch.MetricsCollector.Core.Models;

namespace SkyWatch.MetricsCollector.Core;

public class MetricCollectorService
{
    private readonly ILogger<MetricCollectorService> _logger;
    private readonly IMetricRecordRepository _metricRecordRepository;

    public MetricCollectorService(IMetricRecordRepository metricRecordRepository, ILogger<MetricCollectorService> logger)
    {
        _logger = logger;
        _metricRecordRepository = metricRecordRepository;
    }

    public async Task CreateMetric(MCRequest request)
    {
        _logger.LogInformation("Metrics Collector Service. Create Metric. Start. Request: {@request}", request);
        
        var recordToCreate = new MetricRecord
        {
            Hostname = request.Hostname,
            IpAddress = request.IpAddress,
            MetricName = request.MetricName,
            MetricValue = request.MetricValue,
            TimesTamp = request.TimesTamp
        };
        
        _logger.LogInformation("Metrics Collector Service. Record to create: {@recordToCreate}", recordToCreate);
        await _metricRecordRepository.CreateMetricRecordAsync(recordToCreate);
        _logger.LogInformation("Metrics Collector Service. Metric created.");
    }
}