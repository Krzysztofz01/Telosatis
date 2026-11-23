using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telosatis.Api.Contracts;
using Telosatis.Api.LargeFileTransfer;
using Telosatis.Application.Errors;
using Telosatis.Application.Features.Files.Commands.CreateDirectory;
using Telosatis.Application.Features.Files.Commands.DeleteFile;
using Telosatis.Application.Features.Files.Commands.MoveFile;
using Telosatis.Application.Features.Files.Commands.UploadFile;
using Telosatis.Application.Features.Files.Contracts;
using Telosatis.Application.Features.Files.Queries.DownloadFile;
using Telosatis.Application.Features.Files.Queries.ListDirectory;

namespace Telosatis.Api.Controllers;

[Route("api/files")]
public sealed class FilesController(ISender sender) : BaseController 
{
    [HttpPost("upload")]
    [LargeFileTransfer]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<FileInfoDto>>> UploadFile(
        [FromQuery(Name = "p")] string? directoryPath,
        IFormFile file,
        [FromHeader(Name = "X-Checksum-SHA256")] string checksum, 
        CancellationToken cancellationToken)
    {
        await using var content = file.OpenReadStream();

        var command = new UploadFileCommand(
            directoryPath ?? string.Empty,
            file.FileName,
            checksum,
            content);

        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess
            ? OkFromResult(result)
            : BadRequestFromError<FileInfoDto>(result.Error);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile(
        [FromQuery(Name = "p")] string? filePath,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFileCommand(filePath ?? string.Empty);

        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return result.Error is NotFoundError notFoundError
                ? NotFoundFromError(notFoundError)
                : BadRequestFromError(result.Error);

        return NoContent();
    }

    [HttpPost("move")]
    public async Task<ActionResult<ApiResponse<FileInfoDto>>> MoveFile(
        [FromBody] MoveFileDto body,
        CancellationToken cancellationToken)
    {
        var command = new MoveFileCommand(
            body.SourceFilePath,
            body.DestinationDirectoryPath);

        var result = await sender.Send(command, cancellationToken); 
        if (result.IsFailure)
            return result.Error is NotFoundError notFoundError
                ? NotFoundFromError<FileInfoDto>(notFoundError)
                : BadRequestFromError<FileInfoDto>(result.Error);

        return OkFromResult(result);
    }
   
    [HttpPost("directory")]
    public async Task<ActionResult<ApiResponse<FileInfoDto>>> CreateDirectory(
        [FromQuery(Name = "p")] string? directoryPath,
        CancellationToken cancellationToken)
    {
        var command = new CreateDirectoryCommand(directoryPath ?? string.Empty);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? OkFromResult(result)
            : BadRequestFromError<FileInfoDto>(result.Error);
    }

    [HttpGet("download")]
    [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DownloadFile(
        [FromQuery(Name = "p")] string? filePath,
        CancellationToken cancellationToken)
    {
        var query = new DownloadFileQuery(filePath ?? string.Empty);

        var result = await sender.Send(query, cancellationToken);
        if (result.IsFailure)
            return result.Error is NotFoundError notFoundError
                ? NotFoundFromError(notFoundError)
                : BadRequestFromError(result.Error);

        return FileFromResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FileInfoDto>>>> ListDirectory(
        [FromQuery(Name = "p")] string? directoryPath,
        [FromQuery(Name = "k")] ListDirectoryOrderKey? orderKey,
        [FromQuery(Name = "d")] bool? orderDescending, 
        CancellationToken cancellationToken)
    {
        var query = new ListDirectoryQuery(
            directoryPath ?? string.Empty,
            orderKey,
            orderDescending);

        var result = await sender.Send(query, cancellationToken);
        if (result.IsFailure)
            return result.Error is NotFoundError notFoundError
                ? NotFoundFromError<IReadOnlyList<FileInfoDto>>(notFoundError)
                : BadRequestFromError<IReadOnlyList<FileInfoDto>>(result.Error);

        return Ok(result);
    }
}