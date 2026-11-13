namespace Telosatis.Domain.FileSystem;

public sealed class FileExist
{
    private FileExist(bool exist, bool isDir)
    {
        Exist = exist;
        IsDir = isDir;
    }

    public bool Exist { get; private set; }
    public bool IsDir { get; private set; }

    public static FileExist NotExist => new(false, false);

    public static FileExist File => new(true, false);
    public static FileExist Dir => new(true, true);
}