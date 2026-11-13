namespace Telosatis.Api.Contracts;

public sealed record MoveFileDto(
    string SourceFilePath,
    string DestinationDirectoryPath);