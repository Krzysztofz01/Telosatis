using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Tokens.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Tokens.Queries.IsRootAdminCreated;

public sealed record IsRootAdminCreatedQuery : IQuery<Result<TokenRootAdminCreatedDto>>;