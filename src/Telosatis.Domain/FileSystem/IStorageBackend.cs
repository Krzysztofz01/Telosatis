using Telosatis.Common;

namespace Telosatis.Domain.FileSystem;

public interface IStorageBackend
{
    Task<Result<IReadOnlyCollection<FileInfo>>> Ls(string path, CancellationToken cancellationToken = default);
    Task<Result<FileInfo>> Stat(string path, CancellationToken cancellationToken = default);
    Task<Result<FileExist>> Exist(string path, CancellationToken cancellationToken = default);
    Task<Result<File>> Cat(string path, CancellationToken cancellationToken = default);
    Task<Result<FileInfo>> Mkdir(string path, CancellationToken cancellationToken = default);
    Task<Result> Rm(string path, CancellationToken cancellationToken = default);
    Task<Result<FileInfo>> Mv(string fromFilePath, string toDirectoryPath, CancellationToken cancellationToken = default);
    Task<Result<FileInfo>> Save(string path, Stream stream, CancellationToken cancellationToken = default);
}
