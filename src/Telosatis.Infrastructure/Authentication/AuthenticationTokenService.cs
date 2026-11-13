using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Telosatis.Application.Options;
using Telosatis.Application.Services.Abstractions;
using Telosatis.Domain.Token;
using Telosatis.Infrastructure.Exceptions;

namespace Telosatis.Infrastructure.Authentication;

internal sealed class AuthenticationTokenService(
    IOptions<AuthenticationOptions> authenticationOptions,
    IOptions<ServerOptions> serverOptions,
    TimeProvider timeProvider,
    ILogger<AuthenticationTokenService> logger)
    : IAuthenticationTokenService
{
    private const int TokenExpirationInMinutes = 30;
    
    public async Task<AuthenticationToken> CreateAuthenticationToken(Token token, CancellationToken cancellationToken = default)
    {
        try
        {
            var now = timeProvider.GetUtcNow();
            var expiresAt = now.AddMinutes(TokenExpirationInMinutes);

            var claims = CreateClaimsFromToken(token);

            var credentials = CreateSigningCredentials(authenticationOptions.Value.Key);
        
            var url = serverOptions.Value.Url;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: url,
                audience: url,
                claims: claims,
                notBefore: now.UtcDateTime,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials);
        
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.WriteToken(jwtSecurityToken);

            return await Task.FromResult(new AuthenticationToken(jwtToken, expiresAt));
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("Failed to create the JWT for the specified token.", ex);
        }
    }

    private static IEnumerable<Claim> CreateClaimsFromToken(Token token)
    {
        return
        [
            new Claim(ClaimTypes.NameIdentifier, token.Id.ToString()),
            new Claim(ClaimTypes.Name, token.Name.Value),
            new Claim(ClaimTypes.Role, token.Role.Value.ToString())
        ];
    }

    private static SigningCredentials CreateSigningCredentials(string keyValue)
    {
        var keyBytes = Encoding.UTF8.GetBytes(keyValue);
        
        var key = new SymmetricSecurityKey(keyBytes);
        
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }
}