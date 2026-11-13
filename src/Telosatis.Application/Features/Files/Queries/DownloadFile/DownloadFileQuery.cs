using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;

namespace Telosatis.Application.Features.Files.Queries.DownloadFile;

public sealed record DownloadFileQuery(
    string FilePath) : IQuery<Result<FileDto>>;