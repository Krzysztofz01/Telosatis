using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Files.Commands.CreateDirectory;

public sealed record CreateDirectoryCommand(
    string DirectoryPath) : ICommand<Result<FileInfoDto>>;