using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Files.Commands.MoveFile;

public sealed record MoveFileCommand(
    string SourceFilePath,
    string DestinationDirectoryPath) : ICommand<Result<FileInfoDto>>;