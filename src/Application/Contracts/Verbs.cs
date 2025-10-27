using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.Contracts;

public interface IGetById<in TId, TOut>
{
    Task<TOut> GetAsync(TId id, CancellationToken ct = default);
}

public interface IListItems<TOut>
{
    Task<Paged<TOut>> ListAsync(int page, int size, CancellationToken ct = default);
}

public interface ICreate<in TIn, TOut>
{
    Task<TOut> CreateAsync(TIn input, CancellationToken ct = default);
}

public interface IUpdate<in TId, in TIn, TOut>
{
    Task<TOut> UpdateAsync(TId id, TIn input, CancellationToken ct = default);
}

public interface IDelete<in TId>
{
    Task DeleteAsync(TId id, CancellationToken ct = default);
}
