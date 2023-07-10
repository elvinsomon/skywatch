using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MetricSourceAgent.Services;

public static class PCInformationService
{
    public static IPAddress? GetIpAddress()
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
    
    public static ulong GetTotalMemoryMegabytes()
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

}