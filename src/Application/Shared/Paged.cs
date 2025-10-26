namespace SolidApiExample.Application.Shared;

public sealed class Paged<T>
{
    public required System.Collections.Generic.IReadOnlyList<T> Items { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
    public required int Total { get; init; }
}
