using System.Net;
using System.Net.Sockets;

namespace Telosatis.Api.Extensions;

public static class HttpContextExtensions
{
    public static string ResolveIpAddress(this HttpContext @this)
    {
        ArgumentNullException.ThrowIfNull(@this);
        
        var ip = @this.Connection.RemoteIpAddress;
        if (ip is null) return string.Empty;

        if (ip.IsIPv4MappedToIPv6)
            ip = ip.MapToIPv4();

        if (ip.AddressFamily == AddressFamily.InterNetworkV6)
        {
            var addressBytes = ip.GetAddressBytes();
            Array.Clear(addressBytes, 8, 8);
            return $"{new IPAddress(addressBytes)}/64";
        }

        return ip.ToString();
    }
}