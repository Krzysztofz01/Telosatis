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
        [FromQuery] string? directoryPath,
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
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile(
        [FromQuery] string? filePath,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFileCommand(filePath ?? string.Empty);

        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return result.Error is NotFoundError
                ? NotFound(result.Error)
                : BadRequest(result.Error);

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
            return result.Error is NotFoundError
                ? NotFound(result.Error)
                : BadRequest(result.Error);

        return Ok(result.Value);
    }
   
    [HttpPost("directory")]
    public async Task<ActionResult<ApiResponse<FileInfoDto>>> CreateDirectory(
        [FromQuery] string? directoryPath,
        CancellationToken cancellationToken)
    {
        var command = new CreateDirectoryCommand(directoryPath ?? string.Empty);

        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("download")]
    [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DownloadFile(
        [FromQuery] string? filePath,
        CancellationToken cancellationToken)
    {
        var query = new DownloadFileQuery(filePath ?? string.Empty);

        var result = await sender.Send(query, cancellationToken);
        if (result.IsFailure)
            return result.Error is NotFoundError
                ? NotFound(result.Error)
                : BadRequest(result.Error);

        return File(result.Value);
    }

    [HttpGet]
    public async Task<object> ListDirectory(
        [FromQuery] string? directoryPath,
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
            return result.Error is NotFoundError
                ? NotFound(result.Error)
                : BadRequest(result.Error);

        return Ok(result.Value);
    }
}