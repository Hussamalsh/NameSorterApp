using NameSorterCore.Interfaces;

namespace NameSorterCore.Sorters;

/// <summary>
/// Provides a generic LINQ-based sorting strategy for collections of any type.
/// </summary>
/// <typeparam name="T">The type of elements in the collection to be sorted.</typeparam>
public class LinqSortStrategy<T> : ISortStrategy<T>
{
    private readonly Func<T, object> _primarySortCriteria;
    private readonly Func<T, object> _secondarySortCriteria;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinqSortStrategy{T}"/> class with specified sorting criteria.
    /// </summary>
    /// <param name="primarySortCriteria">The primary sort criteria as a function taking an item of type <typeparamref name="T"/> and returning an object used for primary sorting.</param>
    /// <param name="secondarySortCriteria">The secondary sort criteria as a function taking an item of type <typeparamref name="T"/> and returning an object used for secondary sorting. This parameter is optional.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="primarySortCriteria"/> is null.</exception>
    public LinqSortStrategy(Func<T, object> primarySortCriteria, Func<T, object> secondarySortCriteria = null)
    {
        _primarySortCriteria = primarySortCriteria ?? throw new ArgumentNullException(nameof(primarySortCriteria));
        _secondarySortCriteria = secondarySortCriteria; // It's okay for secondary criteria to be null.
    }

    /// <summary>
    /// Sorts an <see cref="IEnumerable{T}"/> based on the defined primary and optional secondary sorting criteria.
    /// </summary>
    /// <param name="items">The collection of items to sort.</param>
    /// <returns>A new <see cref="IEnumerable{T}"/> that is sorted according to the specified sorting criteria.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is null.</exception>
    public IEnumerable<T> Sort(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        // Start with primary sorting criteria.
        var sortedItems = items.OrderBy(_primarySortCriteria);

        // If secondary sorting criteria is provided, apply it.
        if (_secondarySortCriteria != null)
        {
            sortedItems = sortedItems.ThenBy(_secondarySortCriteria);
        }

        return sortedItems;
    }
}
