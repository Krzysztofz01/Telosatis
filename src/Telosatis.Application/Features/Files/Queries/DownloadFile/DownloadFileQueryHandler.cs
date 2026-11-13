using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;
using Telosatis.Domain.FileSystem;

namespace Telosatis.Application.Features.Files.Queries.DownloadFile;

internal sealed class DownloadFileQueryHandler(
    IStorageBackend storageBackend)
    : IQueryHandler<DownloadFileQuery, Result<FileDto>>
{
    public async Task<Result<FileDto>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        var existResult = await storageBackend.Exist(request.FilePath, cancellationToken);
        if (existResult.IsFailure)
            return existResult.Error;

        if (!existResult.Value.Exist || existResult.Value.IsDir)
            return Errors.FileDoesNotExist;

        var fileResult = await storageBackend.Cat(request.FilePath, cancellationToken);
        if (fileResult.IsFailure)
            return fileResult.Error;

        var file = fileResult.Value;

        return new FileDto(
            new FileInfoDto(
                file.Info.Name,
                file.Info.Path,
                file.Info.Size,
                file.Info.ContentType,
                file.Info.IsDir),
            file.Content);
    }
}