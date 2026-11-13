using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Files.Queries.ListDirectory;

public sealed record ListDirectoryQuery(
    string DirectoryPath,
    ListDirectoryOrderKey? OrderKey,
    bool? OrderDescending) : IQuery<Result<IReadOnlyList<FileInfoDto>>>;

public enum ListDirectoryOrderKey
{
    Name = 1,
    Size = 2,
}
    
    