using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Telosatis.Common;
using Telosatis.Domain.FileSystem;
using File = Telosatis.Domain.FileSystem.File;
using FileInfo = Telosatis.Domain.FileSystem.FileInfo;

namespace Telosatis.Infrastructure.LinuxFileSystem;

internal sealed class LinuxFileSystemStorageBackend : IStorageBackend
{
    private const int ReadBufferSize = 64 * 1024;
    private const int WriteBufferSize = 64 * 1024;

    private static readonly FileExtensionContentTypeProvider FileExtensionContentTypeProvider = new();

    private readonly ILogger<LinuxFileSystemStorageBackend> _logger;

    private readonly string _rootAbsolutePath;
    
    private LinuxFileSystemStorageBackend(
        string rootAbsolutePath,
        ILogger<LinuxFileSystemStorageBackend> logger)
    {
        _rootAbsolutePath = rootAbsolutePath ??
            throw new ArgumentNullException(nameof(rootAbsolutePath));
        
        _logger = logger ??
            throw new ArgumentNullException(nameof(logger));
    }

    public static Result<LinuxFileSystemStorageBackend> CreateStorageBackend(
        string rootAbsolutePath,
        ILoggerFactory loggerFactory)
    {
        var validateRootResult = ValidateRoot(rootAbsolutePath);
        if (validateRootResult.IsFailure) return validateRootResult.Error;

        var logger = loggerFactory.CreateLogger<LinuxFileSystemStorageBackend>();
        return new LinuxFileSystemStorageBackend(rootAbsolutePath, logger);
    }
    
    public async Task<Result<IReadOnlyCollection<FileInfo>>> Ls(string path, CancellationToken cancellationToken = default)
    {
        var resolvedPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, path);
        if (resolvedPathResult.IsFailure)
            return resolvedPathResult.Error;

        var resolvedPath = resolvedPathResult.Value;

        try
        {
            var directoryInfo = new DirectoryInfo(resolvedPath.PhysicalPath);
            var fileInfoEntries = new List<FileInfo>();

            foreach (var entry in directoryInfo.EnumerateFileSystemInfos())
            {
                cancellationToken.ThrowIfCancellationRequested();

                switch (entry)
                {
                    case System.IO.FileInfo file:
                    {
                        var filePath = Path.Combine(resolvedPath.VirtualPath, entry.Name);

                        var fileResult = FileInfo.FromFile(file.Name, filePath, file.Length, ResolveContentType(filePath));
                        if (fileResult.IsFailure)
                            return fileResult.Error;

                        fileInfoEntries.Add(fileResult.Value);
                    }
                        break;
                    case System.IO.DirectoryInfo dir:
                    {
                        var dirPath = Path.Combine(resolvedPath.VirtualPath, entry.Name);

                        var fileResult = FileInfo.FromDir(dir.Name, dirPath);
                        if (fileResult.IsFailure)
                            return fileResult.Error;

                        fileInfoEntries.Add(fileResult.Value);
                    }
                        break;
                    default:
                    {
                        return Errors.UnsupportedDirectoryEntry;
                    }
                }
            }

            return await Task.FromResult(fileInfoEntries);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            LogException(ex, nameof(Ls), path);
            return MapExceptionToError(ex);
        }
    }

    public async Task<Result<FileInfo>> Stat(string path, CancellationToken cancellationToken = default)
    {
        var resolvedPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, path);
        if (resolvedPathResult.IsFailure)
            return resolvedPathResult.Error;
       
        var resolvedPath = resolvedPathResult.Value;

        if (!System.IO.File.Exists(resolvedPath.PhysicalPath))
            return Errors.FileNotFound;

        try
        {
            var file = new System.IO.FileInfo(resolvedPath.PhysicalPath);

            var fileInfoResult = FileInfo.FromFile(file.Name, resolvedPath.VirtualPath, file.Length, ResolveContentType(resolvedPath.VirtualPath));
            if (fileInfoResult.IsFailure)
                return fileInfoResult.Error;

            return await Task.FromResult(fileInfoResult.Value);

        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            LogException(ex, nameof(Stat), path);
            return MapExceptionToError(ex);
        }
    }

    public async Task<Result<FileExist>> Exist(string path, CancellationToken cancellationToken = default)
    {
        var resolvedPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, path);
        if (resolvedPathResult.IsFailure)
            return resolvedPathResult.Error;

        var resolvedPath = resolvedPathResult.Value;

        if (System.IO.File.Exists(resolvedPath.PhysicalPath))
            return await Task.FromResult(FileExist.File);

        if (Directory.Exists(resolvedPath.PhysicalPath))
            return await Task.FromResult(FileExist.Dir);

        return await Task.FromResult(FileExist.NotExist);
    }

    public async Task<Result<File>> Cat(string path, CancellationToken cancellationToken = default)
    {
        var resolvedPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, path);
        if (resolvedPathResult.IsFailure)
            return resolvedPathResult.Error;
       
        var resolvedPath = resolvedPathResult.Value;

        if (!System.IO.File.Exists(resolvedPath.PhysicalPath))
            return Errors.FileNotFound;

        Stream? stream = null;
        try
        {
            var file = new System.IO.FileInfo(resolvedPath.PhysicalPath);
            
            stream = new FileStream(
                resolvedPath.PhysicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: ReadBufferSize,
                options: FileOptions.Asynchronous | FileOptions.SequentialScan);

            var fileResult = File.FromFile(file.Name, resolvedPath.VirtualPath, file.Length, ResolveContentType(resolvedPath.VirtualPath), stream);
            if (fileResult.IsFailure)
            {
                await stream.DisposeAsync();

                return fileResult.Error;
            }

            return await Task.FromResult(fileResult.Value);
        }
        catch (OperationCanceledException)
        {
            if (stream is not null)
                await stream.DisposeAsync();
            
            throw;
        }
        catch (Exception ex)
        {
            if (stream is not null)
                await stream.DisposeAsync();
            
            LogException(ex, nameof(Cat), path);
            return MapExceptionToError(ex);
        }
    }

    public async Task<Result<FileInfo>> Mkdir(string path, CancellationToken cancellationToken = default)
    {
        var resolvedPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, path);
        if (resolvedPathResult.IsFailure)
            return resolvedPathResult.Error;
       
        var resolvedPath = resolvedPathResult.Value;

        if (System.IO.File.Exists(resolvedPath.PhysicalPath))
            return Errors.FileAtThisPathAlreadyExists;
            
        if (Directory.Exists(resolvedPath.PhysicalPath))
            return Errors.DirectoryAtThisPathAlreadyExists;

        try
        {
            var directory = Directory.CreateDirectory(resolvedPath.PhysicalPath);

            var fileInfoResult = FileInfo.FromDir(directory.Name, resolvedPath.VirtualPath);
            if (fileInfoResult.IsFailure)
                return fileInfoResult.Error;

            return await Task.FromResult(fileInfoResult.Value);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            LogException(ex, nameof(Mkdir), path);
            return MapExceptionToError(ex);
        }
    }

    public async Task<Result> Rm(string path, CancellationToken cancellationToken = default)
    {
        var resolvedPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, path);
        if (resolvedPathResult.IsFailure)
            return resolvedPathResult.Error;
       
        var resolvedPath = resolvedPathResult.Value;

        try
        {
            if (System.IO.File.Exists(resolvedPath.PhysicalPath))
            {
                System.IO.File.Delete(resolvedPath.PhysicalPath);
            
                return await Task.FromResult(Result.Success());
            }

            if (Directory.Exists(resolvedPath.PhysicalPath))
            {
                Directory.Delete(resolvedPath.PhysicalPath, true);
            
                return await Task.FromResult(Result.Success());
            }

            return Errors.FileOrDirectoryNotFound;
        } 
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            LogException(ex, nameof(Rm), path);
            return MapExceptionToError(ex);
        }
    }

    public async Task<Result<FileInfo>> Mv(string fromFilePath, string toDirectoryPath, CancellationToken cancellationToken = default)
    {
        var resolvedFromFilePathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, fromFilePath);
        if (resolvedFromFilePathResult.IsFailure)
            return resolvedFromFilePathResult.Error;
       
        var resolvedToDirectoryPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, toDirectoryPath);
        if (resolvedToDirectoryPathResult.IsFailure)
            return resolvedToDirectoryPathResult.Error;
        
        var resolvedFromFilePath = resolvedFromFilePathResult.Value;
        var resolvedToDirectoryPath = resolvedToDirectoryPathResult.Value;

        if (!System.IO.File.Exists(resolvedFromFilePath.PhysicalPath))
            return Errors.SourceFileNotFound;

        if (!Directory.Exists(resolvedToDirectoryPath.PhysicalPath))
            return Errors.DestinationDirectoryNotFound;

        try
        {
            var fileName = Path.GetFileName(resolvedFromFilePath.PhysicalPath);
            var absoluteDestinationFilePath = Path.Combine(resolvedToDirectoryPath.PhysicalPath, fileName);
            var relativeDestinationFilePath = Path.Combine(resolvedToDirectoryPath.VirtualPath, fileName);
            
            if (System.IO.File.Exists(absoluteDestinationFilePath))
                return Errors.DestinationFileAlreadyExists;
         
            System.IO.File.Move(resolvedFromFilePath.PhysicalPath, absoluteDestinationFilePath);

            var file = new System.IO.FileInfo(absoluteDestinationFilePath);

            var fileInfoResult = FileInfo.FromFile(file.Name, relativeDestinationFilePath, file.Length, ResolveContentType(relativeDestinationFilePath));
            if (fileInfoResult.IsFailure)
                return fileInfoResult.Error;
            
            return await Task.FromResult(fileInfoResult.Value);
        } 
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            LogException(ex, nameof(Mv), fromFilePath, toDirectoryPath);
            return MapExceptionToError(ex);
        }
    }

    public async Task<Result<FileInfo>> Save(string path, Stream stream, CancellationToken cancellationToken = default)
    {
        var resolvedPathResult = SandboxedPathResolver.Resolve(_rootAbsolutePath, path);
        if (resolvedPathResult.IsFailure)
            return resolvedPathResult.Error;
       
        var resolvedPath = resolvedPathResult.Value;

        if (System.IO.File.Exists(resolvedPath.PhysicalPath))
            return Errors.DestinationFileAlreadyExists;

        try
        {
            await using var fileStream = new FileStream(
                resolvedPath.PhysicalPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: WriteBufferSize,
                options: FileOptions.Asynchronous | FileOptions.SequentialScan);

            await stream.CopyToAsync(fileStream, cancellationToken);
            
            var file = new System.IO.FileInfo(resolvedPath.PhysicalPath);

            var fileInfoResult = FileInfo.FromFile(file.Name, resolvedPath.VirtualPath, file.Length, ResolveContentType(resolvedPath.VirtualPath));
            if (fileInfoResult.IsFailure)
                return fileInfoResult.Error;

            return await Task.FromResult(fileInfoResult.Value);
        }
        catch (OperationCanceledException)
        {
            EnsureFileDeleted(resolvedPath.PhysicalPath);
            
            throw;
        }
        catch (Exception ex)
        {
            EnsureFileDeleted(resolvedPath.PhysicalPath);
            
            LogException(ex, nameof(Save), path);
            return MapExceptionToError(ex);
        }
    }

    private static Result ValidateRoot(string absoluteRootPath)
    {
        if (string.IsNullOrWhiteSpace(absoluteRootPath))
            return Errors.StorageBackendRootCanNotBeNullOrWhiteSpace;

        if (!Path.IsPathFullyQualified(absoluteRootPath))
            return Errors.StorageBackendRootFormatIsNotValid;

        if (!Directory.Exists(absoluteRootPath))
            return Errors.StorageBackendRootDirectoryDoesNotExist;

        return Result.Success();
    }
    
    private void LogException(Exception exception, string operation, params string[] paths)
    {
        if (paths.Length == 0)
        {
            _logger.LogError(exception, "Operation: {Operation} failed with an exception", operation); 
            return;
        }
        
        var sb = new StringBuilder();
            
        sb.Append("[ ");
        foreach (var (path, index) in paths.Select((x, i) => (x, i)))
        {
            if (index > 0) sb.Append(',');
        
            sb.Append("(\"");
            sb.Append(_rootAbsolutePath);
            sb.Append("\" -> \"");
            sb.Append(path);
            sb.Append("\")");
        }
        sb.Append(" ]");

        _logger.LogError(exception, "Operation: {Operation} failed with an exception with paths: {Paths}", operation, sb.ToString());
    }
    
    private static Error MapExceptionToError(Exception exception)
    {
        return exception switch
        {
            FileNotFoundException => Errors.FileNotFound,
            DirectoryNotFoundException => Errors.DirectoryNotFound,
            UnauthorizedAccessException => Errors.AccessDenied,
            PathTooLongException => Errors.PathTooLong,
            ArgumentNullException => Errors.InvalidPath,
            ArgumentException => Errors.InvalidPath,
            NotSupportedException => Errors.UnsupportedOperation,
            IOException => Errors.IoError,
            _ => Errors.UnexpectedStorageError
        };
    }

    private static bool EnsureFileDeleted(string path)
    {
        try
        {
            if (!System.IO.File.Exists(path))
                return true;
                
            System.IO.File.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static string ResolveContentType(string path)
    {
        return FileExtensionContentTypeProvider.TryGetContentType(path, out var contentType)
            ? contentType
            : "application/octet-stream";
    }
}