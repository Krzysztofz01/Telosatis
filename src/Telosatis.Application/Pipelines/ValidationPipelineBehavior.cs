using FluentValidation;
using MediatR;
using Telosatis.Application.Errors;
using Telosatis.Common;

namespace Telosatis.Application.Pipelines;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, Result<TResponse>> where TRequest : IRequest<Result<TResponse>>
{
    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var validationError = validationResults
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .Where(x => x is not null)
            .OrderBy(x => x.PropertyName)
            .FirstOrDefault();

        if (validationError is null)
            return await next(cancellationToken);

        // TODO: Can we reuse the defined instances?
        return new ValidationError(
            validationError.ErrorCode,
            validationError.ErrorMessage);
    }
}