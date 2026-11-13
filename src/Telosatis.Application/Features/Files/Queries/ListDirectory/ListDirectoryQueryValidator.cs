using FluentValidation;
using Telosatis.Application.Extensions;

namespace Telosatis.Application.Features.Files.Queries.ListDirectory;

public sealed class ListDirectoryQueryValidator : AbstractValidator<ListDirectoryQuery>
{
    public ListDirectoryQueryValidator()
    {
        RuleFor(x => x.DirectoryPath)
            .IsAbsolutePathString()
            .WithError(Errors.PathFormatIsInvalid);

        RuleFor(x => x.OrderKey)
            .IsInEnum()
            .WithError(Errors.ListDirectoryOrderKeyNotSupported);

        RuleFor(x => x.OrderDescending);
    }
}