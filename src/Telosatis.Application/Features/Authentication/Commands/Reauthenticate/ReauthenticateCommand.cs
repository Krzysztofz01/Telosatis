using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Authentication.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Authentication.Commands.Reauthenticate;

public sealed record ReauthenticateCommand(
    Guid TokenId) : ICommand<Result<AuthenticatedDto>>;