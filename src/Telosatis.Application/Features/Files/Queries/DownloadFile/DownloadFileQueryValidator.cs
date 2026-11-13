using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Files.Queries.DownloadFile;

public sealed class DownloadFileQueryValidator : AbstractValidator<DownloadFileQuery>
{
    public DownloadFileQueryValidator()
    {
        RuleFor(x => x.FilePath)
            .IsAbsolutePathString()
            .WithError(Errors.PathFormatIsInvalid);
    }
}