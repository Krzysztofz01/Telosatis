using Telosatis.Common;

namespace Telosatis.Domain.FileSystem;

public sealed class File : IDisposable, IAsyncDisposable
{
    private File(FileInfo info, Stream content)
    {
        Info = info;
        Content = content;
    }

    public FileInfo Info { get; private set; }
    public Stream Content { get; private set; }

    public static Result<File> FromFile(string name, string path, long size, string contentType, Stream stream)
    {
        var fileInfoResult = FileInfo.FromFile(name, path, size, contentType);
        if (fileInfoResult.IsFailure)
            return fileInfoResult.Error;

        if (stream is null || !stream.CanRead)
            return Errors.FileStreamStateIsNotValid;

        return new File(fileInfoResult.Value, stream);
    }
    
    public void Dispose() => Content.Dispose();

    public async ValueTask DisposeAsync() => await Content.DisposeAsync();
}