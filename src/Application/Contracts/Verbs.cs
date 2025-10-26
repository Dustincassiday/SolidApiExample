namespace SolidApiExample.Application.Contracts;


public interface IGetById<TId, TOut>
{
    Task<TOut> GetAsync(TId id, CancellationToken ct = default);
}
public interface IListItems<TOut>
{
    Task<Shared.Paged<TOut>> ListAsync(int page, int size, CancellationToken ct = default);
}
public interface ICreate<TIn, TOut>
{
    Task<TOut> CreateAsync(TIn input, CancellationToken ct = default);
}
public interface IUpdate<TId, TIn, TOut>
{
    Task<TOut> UpdateAsync(TId id, TIn input, CancellationToken ct = default);
}
public interface IDelete<TId>
{
    Task DeleteAsync(TId id, CancellationToken ct = default);
}
