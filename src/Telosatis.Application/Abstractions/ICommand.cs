using MediatR;

namespace Telosatis.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
