namespace SolidApiExample.Application.Shared;

/// <summary>
/// Wraps a paged result set.
/// </summary>
public sealed class Paged<T>
{
    /// <summary>
    /// Items returned for the current page.
    /// </summary>
    public required System.Collections.Generic.IReadOnlyList<T> Items { get; init; }

    /// <summary>
    /// Zero-based index of the current page.
    /// </summary>
    public required int Page { get; init; }

    /// <summary>
    /// Maximum items per page.
    /// </summary>
    public required int Size { get; init; }

    /// <summary>
    /// Total items available across all pages.
    /// </summary>
    public required int Total { get; init; }
}
