using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;
using Telosatis.Domain.FileSystem;

namespace Telosatis.Application.Features.Files.Queries.ListDirectory;

internal sealed class ListDirectoryQueryHandler(
    IStorageBackend storageBackend)
    : IQueryHandler<ListDirectoryQuery, Result<IReadOnlyList<FileInfoDto>>>
{
    public async Task<Result<IReadOnlyList<FileInfoDto>>> Handle(ListDirectoryQuery request, CancellationToken cancellationToken)
    {
        var orderKey = request.OrderKey ?? ListDirectoryOrderKey.Name;
        var orderDescending = request.OrderDescending ?? false;

        var directoryContentResult = await storageBackend.Ls(request.DirectoryPath, cancellationToken);
        if (directoryContentResult.IsFailure)
            return directoryContentResult.Error;

        var directoryContent = directoryContentResult.Value
            .OrderBy(x => x.IsDir);

        directoryContent = orderKey switch
        {
            ListDirectoryOrderKey.Name => orderDescending
                ? directoryContent.ThenByDescending(x => x.Name)
                : directoryContent.ThenBy(x => x.Name),
            ListDirectoryOrderKey.Size => orderDescending
                ? directoryContent.ThenByDescending(x => x.Size)
                : directoryContent.ThenBy(x => x.Size),
            _ => throw new InvalidOperationException("The specified order key is not supported."),
        };

        return directoryContent
            .Select(x => new FileInfoDto(
                x.Name,
                x.Path,
                x.Size,
                x.ContentType,
                x.IsDir))
            .ToList();
    }
}