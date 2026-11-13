using Telosatis.Application.Abstractions;
using Telosatis.Common;
using Telosatis.Domain.FileSystem;

namespace Telosatis.Application.Features.Files.Commands.DeleteFile;

internal sealed class DeleteFileCommandHandler(
    IStorageBackend storageBackend)
    : ICommandHandler<DeleteFileCommand, Result>
{
    public async Task<Result> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var fileExistResult = await storageBackend.Exist(request.FilePath, cancellationToken);
        if (fileExistResult.IsFailure)
            return fileExistResult.Error;

        var fileExist = fileExistResult.Value;

        if (!fileExist.Exist || fileExist.IsDir)
            return Errors.FileDoesNotExist;

        var deleteResult = await storageBackend.Rm(request.FilePath, cancellationToken);
        if (deleteResult.IsFailure)
            return deleteResult.Error;
        
        return Result.Success();
    }
}