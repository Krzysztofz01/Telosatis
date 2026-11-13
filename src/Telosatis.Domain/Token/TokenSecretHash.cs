using System.Security.Cryptography;
using System.Text;
using Telosatis.Common;

namespace Telosatis.Domain.Token;

public sealed record TokenSecret(
    TokenSecretHash SecretHash,
    string Secret);

public sealed class TokenSecretHash : ValueObject<TokenSecretHash>
{
    private const int Version = 1;
    private const string ValuePrefix = "telosatis";
    private const int SecretLength = 32;
    
    public const int HashLength = 128;
    
    public string Value { get; private set; }

    private TokenSecretHash(string value)
    {
        Value = value;
    }

    public static Result<TokenSecret> Create(DateTimeOffset now)
    {
        var secret = BuildSecret(now);
        var secretHash = BuildSecretHash(secret);

        if (secretHash.Length != HashLength)
            return Errors.TokenSecretHashLengthMissmatch;
        
        var tokenSecretHash = new TokenSecretHash(secretHash);
        return new TokenSecret(tokenSecretHash, secret);
    }

    public static Result<TokenSecret> FromSecret(string secret)
    {
        var secretHash = BuildSecretHash(secret);

        if (!secret.StartsWith($"{ValuePrefix}_{Version}_"))
            return Errors.TokenSecretFormatIsInvalid;
            
        var tokenSecretHash = new TokenSecretHash(secretHash);
        
        if (secretHash.Length != HashLength)
            return Errors.TokenSecretHashLengthMissmatch;
        
        return new TokenSecret(tokenSecretHash, secret);
    }

    private static string BuildSecret(DateTimeOffset now)
    {
        Span<byte> data = stackalloc byte[SecretLength];
        RandomNumberGenerator.Fill(data);

        var secretPayloadEncoded= Convert.ToBase64String(data)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

        var timestamp = now.ToUnixTimeMilliseconds();
        
        return $"{ValuePrefix}_{Version}_{timestamp}_{secretPayloadEncoded}";
    }

    private static string BuildSecretHash(string secret)
    {
        var hash = SHA512.HashData(Encoding.UTF8.GetBytes(secret));
        var hashEncoded = Convert.ToHexString(hash);

        return hashEncoded;
    }
}