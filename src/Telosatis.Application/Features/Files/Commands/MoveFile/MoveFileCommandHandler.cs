using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;
using Telosatis.Domain.FileSystem;

namespace Telosatis.Application.Features.Files.Commands.MoveFile;

internal sealed class MoveFileCommandHandler(
    IStorageBackend storageBackend)
    : ICommandHandler<MoveFileCommand, Result<FileInfoDto>>
{
    public async Task<Result<FileInfoDto>> Handle(MoveFileCommand request, CancellationToken cancellationToken)
    {
        var fileExistResult = await storageBackend.Exist(request.SourceFilePath, cancellationToken);
        if (fileExistResult.IsFailure)
            return fileExistResult.Error;

        if (!fileExistResult.Value.Exist || fileExistResult.Value.IsDir)
            return Errors.FileDoesNotExist;

        var dirExistResult = await storageBackend.Exist(request.DestinationDirectoryPath, cancellationToken);
        if (dirExistResult.IsFailure)
            return dirExistResult.Error;

        if (!dirExistResult.Value.Exist || !dirExistResult.Value.IsDir)
            return Errors.DirectoryDoesNotExist;

        var moveResult = await storageBackend.Mv(
            request.SourceFilePath,
            request.DestinationDirectoryPath,
            cancellationToken);

        if (moveResult.IsFailure)
            return moveResult.Error;

        var file = moveResult.Value;

        return new FileInfoDto(
            file.Name,
            file.Path,
            file.Size,
            file.ContentType,
            file.IsDir);
    }
}