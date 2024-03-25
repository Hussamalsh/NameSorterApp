using NameSorterCore.Interfaces;
using NameSorterCore.Models;

namespace NameSorterCore.Sorters;

/// <summary>
/// Implements the sorting of person names using a specified sorting strategy and a name parser.
/// </summary>
public class NameSorter : INameSorter
{
    private readonly ISortStrategy<PersonName> _sortStrategy;
    private readonly INameParser _nameParser;

    /// <summary>
    /// Initializes a new instance of the <see cref="NameSorter"/> class with the specified sorting strategy and name parser.
    /// </summary>
    /// <param name="sortStrategy">The sorting strategy to use for ordering person names.</param>
    /// <param name="nameParser">The parser used to convert string representations of names into <see cref="PersonName"/> objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sortStrategy"/> or <paramref name="nameParser"/> is null.</exception>
    public NameSorter(ISortStrategy<PersonName> sortStrategy, INameParser nameParser)
    {
        _sortStrategy = sortStrategy ?? throw new ArgumentNullException(nameof(sortStrategy));
        _nameParser = nameParser ?? throw new ArgumentNullException(nameof(nameParser));
    }

    /// <summary>
    /// Sorts a list of names represented as strings.
    /// </summary>
    /// <param name="names">The list of names to sort.</param>
    /// <returns>A sorted list of <see cref="PersonName"/> objects, ordered according to the specified sorting strategy.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="names"/> argument is null.</exception>
    /// <remarks>
    /// This method first parses the input list of name strings into <see cref="PersonName"/> objects using the provided <see cref="INameParser"/>.
    /// It then sorts the parsed names using the provided <see cref="ISortStrategy{PersonName}"/>.
    /// </remarks>
    public List<PersonName> Sort(List<string> names)
    {
        if (names == null) throw new ArgumentNullException(nameof(names));

        var parsedNames = names.Select(name => _nameParser.Parse(name)).ToList();

        // Utilize the generic sort strategy, ensuring it's explicitly for PersonName.
        var sortedNames = _sortStrategy.Sort(parsedNames);

        return sortedNames.ToList();
    }
}
