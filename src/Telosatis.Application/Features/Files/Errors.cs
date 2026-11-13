using Telosatis.Application.Errors;

namespace Telosatis.Application.Features.Files;

// FIXME: Implement
public static class Errors
{
    public static readonly OperationError DirectoryAtGivenPathAlreadyExists = new("", "");
    public static readonly OperationError FileAtGivenPathAlreadyExists = new("", "");
    public static readonly OperationError FileDoesNotExist = new("", "");
    public static readonly OperationError DirectoryDoesNotExist = new("", "");
    public static readonly OperationError  FileUploadChecksumFormatIsInvalid = new("", "");
    public static readonly OperationError  FileUploadChecksumMismatch = new("", "");
    
    public static readonly ValidationError PathFormatIsInvalid = new("", "");
    public static readonly ValidationError FileNameCanNotBeEmpty = new("", "");
    public static readonly ValidationError FileContentCanNotBeNull = new("", "");
    public static readonly ValidationError ListDirectoryOrderKeyNotSupported = new("", "");
    public static readonly ValidationError FileChecksumCanNotBeEmpty = new("", "");
}