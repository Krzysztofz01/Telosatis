using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Files.Commands.UploadFile;

public sealed record UploadFileCommand(
    string DirectoryPath,
    string Name,
    string Checksum,
    Stream Content) : ICommand<Result<FileInfoDto>>;