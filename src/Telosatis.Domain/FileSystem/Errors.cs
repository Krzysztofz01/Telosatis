using Telosatis.Common;

namespace Telosatis.Domain.FileSystem;

// FIXME: Implement
public static class Errors
{
    public static readonly Error RootPathCanNotBeNullOrWhiteSpace = new("", "");
    public static readonly Error VirtualPathCanNotBeNullOrWhiteSpace = new("", "");
    public static readonly Error VirtualPathContainsBlockedCharacters = new("", "");
    public static readonly Error VirtualPathContainsNavigationSegments = new("", "");
    public static readonly Error ResolvedPhysicalPathRootMismatch = new("", "");
    public static readonly Error UnsupportedDirectoryEntry = new("", "");
    public static readonly Error FileNotFound = new("", "");
    public static readonly Error DirectoryNotFound = new("", "");
    public static readonly Error AccessDenied = new("", "");
    public static readonly Error PathTooLong = new("", "");
    public static readonly Error InvalidPath = new("", "");
    public static readonly Error UnsupportedOperation = new("", "");
    public static readonly Error IoError = new("", "");
    public static readonly Error UnexpectedStorageError = new("", "");
    public static readonly Error FileAtThisPathAlreadyExists = new("", "");
    public static readonly Error DirectoryAtThisPathAlreadyExists = new("", "");
    public static readonly Error FileOrDirectoryNotFound = new("", "");
    public static readonly Error SourceFileNotFound = new("", "");
    public static readonly Error DestinationDirectoryNotFound = new("", "");
    public static readonly Error DestinationFileAlreadyExists = new("", "");
    public static readonly Error FileNameCanNotBeEmpty = new("", "");
    public static readonly Error FilePathCanNotBeEmpty = new("", "");
    public static readonly Error FileSizeCanNotBeNegative = new("", "");
    public static readonly Error FilePathRootCanNotBeNullOrWhitespace = new("", "");
    public static readonly Error FilePathRelativePathCanNotBeNullOrWhitespace = new("", "");
    public static readonly Error FileStreamStateIsNotValid = new("", "");
    public static readonly Error ResolvedVirtualRelativePathCanNotBeRooted = new("", "");
    public static readonly Error ResolvedPhysicalPathIsNotRooted = new("", "");
    public static readonly Error StorageBackendRootCanNotBeNullOrWhiteSpace = new("", "");
    public static readonly Error StorageBackendRootFormatIsNotValid = new("", "");
    public static readonly Error StorageBackendRootDirectoryDoesNotExist = new("", "");
    public static readonly Error RootPathIsNotRooted = new("", "");
    public static readonly Error VirtualPathIsNotRooted = new("", "");
}