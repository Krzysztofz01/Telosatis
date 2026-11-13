using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Files.Commands.MoveFile;

public sealed class MoveFileCommandValidator : AbstractValidator<MoveFileCommand>
{
    public MoveFileCommandValidator()
    {
        RuleFor(x => x.SourceFilePath)
            .IsAbsolutePathString()
            .WithError(Errors.PathFormatIsInvalid);

        RuleFor(x => x.DestinationDirectoryPath)
            .IsAbsolutePathString()
            .WithError(Errors.PathFormatIsInvalid);
    }
}