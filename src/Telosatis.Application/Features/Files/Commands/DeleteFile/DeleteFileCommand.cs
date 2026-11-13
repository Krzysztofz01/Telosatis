using Telosatis.Application.Abstractions;
using Telosatis.Common;

namespace Telosatis.Application.Features.Files.Commands.DeleteFile;

public sealed record DeleteFileCommand(
    string FilePath) : ICommand<Result>;