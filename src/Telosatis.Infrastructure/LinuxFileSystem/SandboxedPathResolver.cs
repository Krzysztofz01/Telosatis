using Telosatis.Common;
using Telosatis.Domain.FileSystem;

namespace Telosatis.Infrastructure.LinuxFileSystem;

internal sealed record SandboxedPath(
    string PhysicalPath,
    string VirtualPath);

internal static class SandboxedPathResolver
{
    private const char Separator = '/';
    private static readonly HashSet<string> NavigationSegments = [".", ".."];
    private static readonly HashSet<char> BlockedCharacters = ['\0', '\\', ':'];

    public static Result<SandboxedPath> Resolve(string absoluteRootPath, string absoluteVirtualPath)
    {
        if (string.IsNullOrWhiteSpace(absoluteRootPath))
            return Errors.RootPathCanNotBeNullOrWhiteSpace;

        if (!IsPathRooted(absoluteRootPath))
            return Errors.RootPathIsNotRooted;
        
        if (string.IsNullOrWhiteSpace(absoluteVirtualPath))
            return Errors.VirtualPathCanNotBeNullOrWhiteSpace;

        if (!IsPathRooted(absoluteVirtualPath))
            return Errors.VirtualPathIsNotRooted;
        
        if (absoluteVirtualPath.Any(x => BlockedCharacters.Contains(x)))
            return Errors.VirtualPathContainsBlockedCharacters;

        var virtualPathSegments = new List<string>();
        foreach (var segment in absoluteVirtualPath.Split(Separator, StringSplitOptions.RemoveEmptyEntries))
        {
            if (NavigationSegments.Contains(segment))
                return Errors.VirtualPathContainsNavigationSegments;

            virtualPathSegments.Add(segment);
        }
        
        var resolvedRootPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(absoluteRootPath));
        var resolvedRootPathWithSeparator = resolvedRootPath + Separator;

        var resolvedVirtualRelativePath = string.Join(Separator, virtualPathSegments);
        if (IsPathRooted(resolvedVirtualRelativePath))
            return Errors.ResolvedVirtualRelativePathCanNotBeRooted;

        var resolvedVirtualPath = Separator + resolvedVirtualRelativePath;
        if (!IsPathRooted(resolvedVirtualPath))
            return Errors.ResolvedVirtualRelativePathCanNotBeRooted;
        
        var resolvedPhysicalPath = Path.GetFullPath(Path.Combine(resolvedRootPath, resolvedVirtualRelativePath));
        if (resolvedPhysicalPath != resolvedRootPath && !resolvedPhysicalPath.StartsWith(resolvedRootPathWithSeparator, StringComparison.Ordinal))
            return Errors.ResolvedPhysicalPathRootMismatch;

        if (!IsPathRooted(resolvedPhysicalPath))
            return Errors.ResolvedPhysicalPathIsNotRooted;
        
        return new SandboxedPath(resolvedPhysicalPath, resolvedVirtualPath);
    }

    private static bool IsPathRooted(string path)
    {
        return path.Length > 0 && path[0] == Separator && Path.IsPathRooted(path);
    }
}