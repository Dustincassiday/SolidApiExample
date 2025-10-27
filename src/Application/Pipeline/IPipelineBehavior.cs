namespace SolidApiExample.Application.Pipeline;

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken ct, Func<CancellationToken, Task<TResponse>> next);
}
