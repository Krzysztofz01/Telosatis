using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Files.Commands.CreateDirectory;

public sealed class CreateDirectoryCommandValidator : AbstractValidator<CreateDirectoryCommand>
{
    public CreateDirectoryCommandValidator()
    {
        RuleFor(x => x.DirectoryPath)
            .IsAbsolutePathString()
            .WithError(Errors.PathFormatIsInvalid);
    }
}