namespace Telosatis.Application.Features.Files.Contracts;

public sealed record FileInfoDto(
    string Name,
    string Path,
    long Size,
    string ContentType,
    bool IsDir);