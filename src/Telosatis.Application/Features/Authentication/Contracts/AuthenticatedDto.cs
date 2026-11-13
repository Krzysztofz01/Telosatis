namespace Telosatis.Application.Features.Authentication.Contracts;

public sealed record AuthenticatedDto(
    string BearerToken,
    DateTimeOffset ExpiresAt);