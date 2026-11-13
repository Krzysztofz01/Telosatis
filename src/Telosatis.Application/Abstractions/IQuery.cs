using MediatR;

namespace Telosatis.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
