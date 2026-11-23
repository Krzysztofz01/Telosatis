using Telosatis.Application.Errors;

namespace Telosatis.Application.Features.Files;

public static class Errors
{
    public static readonly OperationError DirectoryAtGivenPathAlreadyExists = new("AFS0001", "Directory at given path already exists.");
    public static readonly OperationError FileAtGivenPathAlreadyExists = new("AFS0002", "File at given path already exists.");
    public static readonly OperationError FileDoesNotExist = new("AFS0003", "File does not exist.");
    public static readonly OperationError DirectoryDoesNotExist = new("AFS0004", "Directory does not exist.");
    public static readonly OperationError FileUploadChecksumFormatIsInvalid = new("AFS0005", "File upload checksum format is not valid.");
    public static readonly OperationError FileUploadChecksumMismatch = new("AFS0006", "File upload checksum does not match the file content.");
    
    public static readonly ValidationError PathFormatIsInvalid = new("AFS0007", "Path format is not valid.");
    public static readonly ValidationError FileNameCanNotBeEmpty = new("AFS0008", "File name cannot be null or empty.");
    public static readonly ValidationError FileContentCanNotBeNull = new("AFS0009", "File content cannot be null.");
    public static readonly ValidationError ListDirectoryOrderKeyNotSupported = new("AFS0010", "Directory listing order key is not supported.");
    public static readonly ValidationError FileChecksumCanNotBeEmpty = new("AFS0011", "File checksum cannot be null or empty.");
}