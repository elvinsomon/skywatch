using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using MetricSourceAgent.Services;
using MetricsShared.Common;
using MetricsShared.MetricsCollectorDTO;

Console.WriteLine("MetricSourceAgent. Starting...");

// Collect PC information
Console.Write("Ingrese el nombre del host: ");
var hostname = Console.ReadLine();

var _ipv4Address = PCInformationService.GetIpAddress();
var totalMemoryMegabytes = PCInformationService.GetTotalMemoryMegabytes();

var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
var cpuCounter = new PerformanceCounter("Process", "% Processor Time", "_Total");

// Send metrics to MetricCollector API
var metricsCollectorApiClient = new MetricsCollectorApiClient(hostname, _ipv4Address!, "");

while (true)
{
    var cpu = cpuCounter.NextValue();
    var availableRam = ramCounter.NextValue();
    var consumedRam = totalMemoryMegabytes - availableRam;

    Console.WriteLine("RAM: " + (consumedRam) + " MB; CPU: " + (cpu / 10) + " %");
    Console.WriteLine("MetricSourceAgent. Invoke API");

    Task.Run(async () => { await metricsCollectorApiClient.InvokeAsync(MetricType.ram_consumed, consumedRam); });
    Task.Run(async () => { await metricsCollectorApiClient.InvokeAsync(MetricType.cpu_usage, cpu / 10); });

    Thread.Sleep(5000);
}