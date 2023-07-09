using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using MetricsShared.Common;
using MetricsShared.MetricsCollectorDTO;

Console.WriteLine("MetricSourceAgent. Starting...");

Console.Write("Ingrese el nombre del host: ");
var _hostname = Console.ReadLine();

var _ipv4Address = GetIpAddress();
var totalMemoryMegabytes = GetTotalMemoryMegabytes();

var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
var cpuCounter = new PerformanceCounter("Process", "% Processor Time", "_Total");

while (true)
{
    var cpu = cpuCounter.NextValue();
    var availableRam = ramCounter.NextValue();
    var consumedRam = totalMemoryMegabytes - availableRam;
    
    Console.WriteLine("RAM: " + (consumedRam) + " MB; CPU: " + (cpu / 10) + " %");
    Console.WriteLine("MetricSourceAgent. Invoke API");
    
    Task.Run(async () => { await InvokeMetricCollectorApi(MetricType.ram_consumed, consumedRam); });
    Task.Run(async () => { await InvokeMetricCollectorApi(MetricType.cpu_usage, cpu / 10); });
    
    Thread.Sleep(5000);
}

// Methods

ulong GetTotalMemoryMegabytes()
{
    var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
    ulong totalMemoryBytes = 0;

    foreach (var obj in searcher.Get())
    {
        var capacityBytes = Convert.ToUInt64(obj["TotalPhysicalMemory"]);
        totalMemoryBytes += capacityBytes;
    }

    return totalMemoryBytes / (1024 * 1024);
}

async Task InvokeMetricCollectorApi(MetricType metricType, float metricValue)
{
    try
    {
        var metricName = Enum.GetName(metricType);
        Console.WriteLine($"Invocando API para métrica {metricName} com valor {metricValue}");
        
        using var httpClient = new HttpClient();
        var request = new MCRequest
        {
            Hostname = _hostname,
            IpAddress = _ipv4Address?.ToString(),
            MetricName = metricName,
            MetricValue = metricValue.ToString(),
            TimesTamp = DateTime.Now.ToString()
        };
    
        var response = await httpClient.PostAsJsonAsync("http://localhost:5092/api/MetricCollector/createMetric", request);
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response from Task {Task.CurrentId}: {responseBody}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error invoking API");
        Console.WriteLine(ex.Message);
    }
}

IPAddress? GetIpAddress()
{
    var interfaces = NetworkInterface.GetAllNetworkInterfaces();
    var ipAddress = interfaces.FirstOrDefault(
        iface => iface.OperationalStatus == OperationalStatus.Up
                 && iface.NetworkInterfaceType != NetworkInterfaceType.Loopback
                 && iface.GetIPProperties().GatewayAddresses.Any()
                 && iface.GetIPProperties().UnicastAddresses.Any(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
    )?.GetIPProperties().UnicastAddresses.FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
    return ipAddress;
}