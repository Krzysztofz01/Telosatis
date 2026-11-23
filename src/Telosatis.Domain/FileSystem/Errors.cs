using Telosatis.Common;

namespace Telosatis.Domain.FileSystem;

public static class Errors
{
    public static readonly Error RootPathCanNotBeNullOrWhiteSpace = new("DFS0001", "Root path cannot be null or empty.");
    public static readonly Error VirtualPathCanNotBeNullOrWhiteSpace = new("DFS0002", "Virtual path cannot be null or empty.");
    public static readonly Error VirtualPathContainsBlockedCharacters = new("DFS0003", "Virtual path contains blocked characters.");
    public static readonly Error VirtualPathContainsNavigationSegments = new("DFS0004", "Virtual path cannot contain navigation segments ('.' or '..').");
    public static readonly Error ResolvedPhysicalPathRootMismatch = new("DFS0005", "Resolved physical path is outside the configured root path.");
    public static readonly Error UnsupportedDirectoryEntry = new("DFS0006", "Directory entry type is not supported.");
    public static readonly Error FileNotFound = new("DFS0007", "File was not found.");
    public static readonly Error DirectoryNotFound = new("DFS0008", "Directory was not found.");
    public static readonly Error AccessDenied = new("DFS0009", "Access to the path is denied.");
    public static readonly Error PathTooLong = new("DFS0010", "Path exceeds the maximum allowed length.");
    public static readonly Error InvalidPath = new("DFS0011", "Path is not valid.");
    public static readonly Error UnsupportedOperation = new("DFS0012", "Operation is not supported.");
    public static readonly Error IoError = new("DFS0013", "An I/O error occurred while accessing the storage.");
    public static readonly Error UnexpectedStorageError = new("DFS0014", "An unexpected storage error occurred.");
    public static readonly Error FileAtThisPathAlreadyExists = new("DFS0015", "A file already exists at this path.");
    public static readonly Error DirectoryAtThisPathAlreadyExists = new("DFS0016", "A directory already exists at this path.");
    public static readonly Error FileOrDirectoryNotFound = new("DFS0017", "File or directory was not found.");
    public static readonly Error SourceFileNotFound = new("DFS0018", "Source file was not found.");
    public static readonly Error DestinationDirectoryNotFound = new("DFS0019", "Destination directory was not found.");
    public static readonly Error DestinationFileAlreadyExists = new("DFS0020", "Destination file already exists.");
    public static readonly Error FileNameCanNotBeEmpty = new("DFS0021", "File name cannot be empty.");
    public static readonly Error FilePathCanNotBeEmpty = new("DFS0022", "File path cannot be empty.");
    public static readonly Error FileSizeCanNotBeNegative = new("DFS0023", "File size cannot be negative.");
    public static readonly Error FileStreamStateIsNotValid = new("DFS0024", "File stream is not in a valid state.");
    public static readonly Error ResolvedVirtualRelativePathCanNotBeRooted = new("DFS0025", "Resolved virtual relative path cannot be rooted.");
    public static readonly Error ResolvedPhysicalPathIsNotRooted = new("DFS0026", "Resolved physical path must be rooted.");
    public static readonly Error StorageBackendRootCanNotBeNullOrWhiteSpace = new("DFS0027", "Storage backend root cannot be null or empty.");
    public static readonly Error StorageBackendRootFormatIsNotValid = new("DFS0028", "Storage backend root format is not valid.");
    public static readonly Error StorageBackendRootDirectoryDoesNotExist = new("DFS0029", "Storage backend root directory does not exist.");
    public static readonly Error RootPathIsNotRooted = new("DFS0030", "Root path must be an absolute (rooted) path.");
    public static readonly Error VirtualPathIsNotRooted = new("DFS0031", "Virtual path must be rooted.");
}