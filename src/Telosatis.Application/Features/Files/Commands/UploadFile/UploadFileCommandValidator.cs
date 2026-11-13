using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Files.Commands.UploadFile;

public sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.DirectoryPath)
            .IsAbsolutePathString()
            .WithError(Errors.PathFormatIsInvalid);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.FileNameCanNotBeEmpty);

        RuleFor(x => x.Checksum)
            .NotEmpty()
            .WithError(Errors.FileChecksumCanNotBeEmpty);
        
        RuleFor(x => x.Content)
            .NotNull()
            .WithError(Errors.FileContentCanNotBeNull);
    }
}