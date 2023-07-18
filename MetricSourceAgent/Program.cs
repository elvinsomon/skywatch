using System.Diagnostics;
using System.Net;
using MetricSourceAgent.Services;
using MetricsShared.Common;
using Microsoft.Extensions.Configuration;

internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("MetricSourceAgent. Starting...");

            var collectorApiUrl = GetCollectorApiUrl();
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            var cpuCounter = new PerformanceCounter("Process", "% Processor Time", "_Total");
            GetHostInfo(out var hostname, out var ipv4Address, out var totalMemoryMegabytes);
            var metricsCollectorApiClient = new MetricsCollectorApiClient(hostname, ipv4Address!, collectorApiUrl!);

            while (true)
            {
                var cpu = cpuCounter.NextValue();
                var availableRam = ramCounter.NextValue();
                var consumedRam = totalMemoryMegabytes - availableRam;

                Console.WriteLine("RAM: " + (consumedRam) + " MB; CPU: " + (cpu / 10) + " %");
                Console.WriteLine("MetricSourceAgent. Invoke API");

                Task.Run(async () => { await metricsCollectorApiClient.InvokeAsync(MetricType.ram_consumed, consumedRam); });
                Task.Run(async () => { await metricsCollectorApiClient.InvokeAsync(MetricType.cpu_usage, cpu / 10); });

                Thread.Sleep(2000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Collecting Metrics");
            Console.WriteLine(ex.Message);
        }
    }

    private static string? GetCollectorApiUrl()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var collectorApiUrl = configuration.GetSection("MetricsCollectorApiUrl").Value;
        return collectorApiUrl;
    }

    private static void GetHostInfo(out string hostName, out IPAddress? ipAddress, out ulong totalMemoryMegabytes1)
    {
        hostName = Environment.MachineName;
        ipAddress = PCInformationService.GetIpAddress();
        totalMemoryMegabytes1 = PCInformationService.GetTotalMemoryMegabytes();
    }
}