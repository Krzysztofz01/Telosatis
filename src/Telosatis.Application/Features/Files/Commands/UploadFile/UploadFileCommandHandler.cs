using System.Security.Cryptography;
using Telosatis.Application.Abstractions;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Common;
using Telosatis.Domain.FileSystem;

namespace Telosatis.Application.Features.Files.Commands.UploadFile;

internal sealed class UploadFileCommandHandler(
    IStorageBackend storageBackend)
    : ICommandHandler<UploadFileCommand, Result<FileInfoDto>>
{
    public async Task<Result<FileInfoDto>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var dirExistResult = await storageBackend.Exist(request.DirectoryPath, cancellationToken);
        if (dirExistResult.IsFailure)
            return dirExistResult.Error;

        if (!dirExistResult.Value.Exist || !dirExistResult.Value.IsDir)
            return Errors.DirectoryDoesNotExist;

        var filePath = Path.Combine(request.DirectoryPath, request.Name);
        var fileResult = await storageBackend.Save(filePath, request.Content, cancellationToken);
        if (fileResult.IsFailure)
            return fileResult.Error;

        var verifyChecksumResult = await VerifyChecksum(filePath, request.Checksum, cancellationToken);
        if (verifyChecksumResult.IsFailure)
            return verifyChecksumResult.Error;
        
        var file = fileResult.Value;

        return new FileInfoDto(
            file.Name,
            file.Path,
            file.Size,
            file.ContentType,
            file.IsDir);
    }

    private async Task<Result> VerifyChecksum(string filePath, string checksum, CancellationToken cancellationToken)
    {
        Stream? file = null;
        try
        {
            var expectedChecksumResult = GetChecksumBytes(checksum);
            if (expectedChecksumResult.IsFailure)
                return expectedChecksumResult.Error;

            var expectedChecksum = expectedChecksumResult.Value;
            
            var uploadedFileResult = await storageBackend.Cat(filePath, cancellationToken);
            if (uploadedFileResult.IsFailure)
                return uploadedFileResult.Error;

            file = uploadedFileResult.Value.Content;
            
            var actualChecksum = await SHA256.HashDataAsync(file, cancellationToken);

            if (expectedChecksum.Length != 32 ||
                actualChecksum.Length != 32 ||
                !CryptographicOperations.FixedTimeEquals(actualChecksum, expectedChecksum))
                return Errors.FileUploadChecksumMismatch;

            return Result.Success();
        }
        finally
        {
            if (file is not null)
                await file.DisposeAsync();
        }
    }

    private static Result<byte[]> GetChecksumBytes(string checksum)
    {
        try
        {
            return Convert.FromHexString(checksum);
        }
        catch
        {
            return Errors.FileUploadChecksumFormatIsInvalid;
        }
    }
}