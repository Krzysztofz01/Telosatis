using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Files.Commands.DeleteFile;

public sealed class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(x => x.FilePath)
            .IsAbsolutePathString()
            .WithError(Errors.PathFormatIsInvalid);
    }
}