using Telosatis.Common;

namespace Telosatis.Domain.FileSystem;

public sealed class FileInfo
{
    private FileInfo(string name, string path, long size, string contentType, bool isDir)
    {
        Name = name;
        Path = path;
        Size = size;
        ContentType = contentType;
        IsDir = isDir;
    }

    public string Name { get; private set; }
    public string Path { get; private set; }
    public long Size { get; private set; }
    public string ContentType { get; private set; }
    public bool IsDir { get; private set; }

    public static Result<FileInfo> FromFile(string name, string path, long size, string contentType)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.FileNameCanNotBeEmpty;

        if (string.IsNullOrWhiteSpace(path))
            return Errors.FilePathCanNotBeEmpty;

        if (size < 0)
            return Errors.FileSizeCanNotBeNegative;

        return new FileInfo(
            name,
            path,
            size,
            contentType,
            false);
    }

    public static Result<FileInfo> FromDir(string name, string path)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.FileNameCanNotBeEmpty;

        if (string.IsNullOrWhiteSpace(path))
            return Errors.FilePathCanNotBeEmpty;

        return new FileInfo(
            name,
            path,
            0,
            string.Empty,
            false);
    }
}