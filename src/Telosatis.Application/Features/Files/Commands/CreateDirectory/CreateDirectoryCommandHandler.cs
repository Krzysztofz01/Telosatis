using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;
using Telosatis.Domain.FileSystem;

namespace Telosatis.Application.Features.Files.Commands.CreateDirectory;

internal sealed class CreateDirectoryCommandHandler(
    IStorageBackend storageBackend)
    : ICommandHandler<CreateDirectoryCommand, Result<FileInfoDto>>
{
    public async Task<Result<FileInfoDto>> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
    {
        var fileExistResult = await storageBackend.Exist(request.DirectoryPath, cancellationToken);
        if (fileExistResult.IsFailure)
            return fileExistResult.Error;

        var fileExist = fileExistResult.Value;
        
        if (fileExist.Exist)
        {
            return fileExist.IsDir
                ? Errors.DirectoryAtGivenPathAlreadyExists
                : Errors.FileAtGivenPathAlreadyExists;
        }

        var dirResult = await storageBackend.Mkdir(request.DirectoryPath, cancellationToken);
        if (dirResult.IsFailure)
            return dirResult.Error;

        var dir = dirResult.Value;

        return new FileInfoDto(
            dir.Name,
            dir.Path,
            dir.Size,
            dir.ContentType,
            dir.IsDir);
    }
}