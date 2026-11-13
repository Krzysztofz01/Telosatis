namespace Telosatis.Application.Features.Tokens.Contracts;

public sealed record TokenCreatedDto(
    TokenDto Token,
    string Secret);