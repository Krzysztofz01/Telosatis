using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Authentication.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Authentication.Commands.Authenticate;

public sealed record AuthenticateCommand(
    string Secret) : ICommand<Result<AuthenticatedDto>>;