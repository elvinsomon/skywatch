using System.Net;
using System.Net.Http.Json;
using MetricsShared.Common;
using MetricsShared.MetricsCollectorDTO;

namespace MetricSourceAgent.Services;

public class MetricsCollectorApiClient
{
    private readonly string _hostname;
    private readonly IPAddress _ipv4Address;
    private readonly string _apiUrl; 

    public MetricsCollectorApiClient(string hostname, IPAddress ipv4Address, string apiUrl)
    {
        _hostname = hostname;
        _ipv4Address = ipv4Address;
        _apiUrl = apiUrl;
    }

    public async Task InvokeAsync(MetricType metricType, float metricValue)
    {
        try
        {
            var request = GenerateRequest(metricType, metricValue, _hostname, _ipv4Address);
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync(_apiUrl, request);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Metric Send Successfully. Response from Task {Task.CurrentId}: {responseBody}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error invoking API");
            Console.WriteLine(ex.Message);
        }
    }

    private static MCRequest GenerateRequest(MetricType metricType, float metricValue, string? hostname, IPAddress? ipv4Address)
    {
        var metricName = Enum.GetName(metricType);
        Console.WriteLine($"Invocando API para métrica {metricName} com valor {metricValue}");

        var mcRequest = new MCRequest
        {
            Hostname = hostname,
            IpAddress = ipv4Address?.ToString(),
            MetricName = metricName,
            MetricValue = metricValue.ToString(),
            TimesTamp = DateTime.UtcNow
        };

        return mcRequest;
    }
}