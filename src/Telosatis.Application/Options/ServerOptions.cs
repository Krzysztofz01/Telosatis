using System.Net;
using Telosatis.Common;

namespace Telosatis.Application.Options;

public sealed class ServerOptions : IValidableOptions
{
    public required string Url { get; init; }
    public required IReadOnlyCollection<string>? TrustedProxies { get; init; }
    public required IReadOnlyCollection<string>? TrustedNetworks { get; init; }
    
    public bool IsValid()
    {
        if (!IsUrlValid(Url)) return false;

        if (TrustedProxies is not null && !TrustedProxies.All(x => IPAddress.TryParse(x, out _))) return false;
        
        if (TrustedNetworks is not null && !TrustedNetworks.All(x => IPNetwork.TryParse(x, out _))) return false;
        
        return true;
    }

    private static bool IsUrlValid(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            && !string.IsNullOrWhiteSpace(uri.Host);
}