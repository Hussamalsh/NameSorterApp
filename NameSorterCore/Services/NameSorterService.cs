using NameSorterCore.Interfaces;
using NameSorterCore.Models;

namespace NameSorterCore.Services;

/// <summary>
/// Provides services to sort a list of names based on defined criteria.
/// This service validates the names using an <see cref="INameValidator"/> and sorts them with an <see cref="INameSorter"/>.
/// </summary>
public class NameSorterService : INameSorterService
{
    private readonly INameValidator _nameValidator;
    private readonly INameSorter _nameSorter;

    /// <summary>
    /// Initializes a new instance of the <see cref="NameSorterService"/> class.
    /// </summary>
    /// <param name="nameValidator">The service used to validate names.</param>
    /// <param name="nameSorter">The service used to sort the validated names.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="nameValidator"/> or <paramref name="nameSorter"/> is null.</exception>
    public NameSorterService(INameValidator nameValidator, INameSorter nameSorter)
    {
        _nameValidator = nameValidator ?? throw new ArgumentNullException(nameof(nameValidator));
        _nameSorter = nameSorter ?? throw new ArgumentNullException(nameof(nameSorter));
    }

    /// <summary>
    /// Sorts a list of names asynchronously after validating them.
    /// </summary>
    /// <param name="unsortedNames">The list of unsorted names to process.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="PersonName"/> objects that are sorted.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="unsortedNames"/> is null.</exception>
    public async Task<List<PersonName>> SortNamesAsync(List<string> unsortedNames, CancellationToken cancellationToken = default)
    {
        // Handle null input by throwing an ArgumentNullException
        if (unsortedNames == null) throw new ArgumentNullException(nameof(unsortedNames));

        // Proceed only if there are names to process, otherwise return an empty list immediately.
        if (unsortedNames.Count == 0) return new List<PersonName>();

        // Ensuring the cancellation request is respected
        cancellationToken.ThrowIfCancellationRequested();

        var validNames = _nameValidator.ValidateNames(unsortedNames);
        // Even if all names are invalid, ensure a consistent return type
        if (validNames.Count == 0) return new List<PersonName>();

        return _nameSorter.Sort(validNames);
    }
}
