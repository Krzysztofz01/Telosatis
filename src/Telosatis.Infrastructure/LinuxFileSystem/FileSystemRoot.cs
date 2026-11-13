namespace Telosatis.Infrastructure.LinuxFileSystem;

internal static class FileSystemRoot
{
    public const string RootPath = "/tmp/telosatis/data";
    
    public static bool EnsureRootPath()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(RootPath))
                return false;

            if (!Path.IsPathFullyQualified(RootPath))
                return false;

            if (!Directory.Exists(RootPath))
                Directory.CreateDirectory(RootPath);
            
            return Directory.Exists(RootPath);
        }
        catch
        {
            return false;
        }
    }
}