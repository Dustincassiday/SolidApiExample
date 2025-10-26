using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidApiExample.Application.Pipeline;

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken ct, Func<CancellationToken, Task<TResponse>> next);
}
