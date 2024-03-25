using NameSorterCore.Models;

namespace NameSorterCore.Interfaces;

public interface INameSorterService
{
    Task<List<PersonName>> SortNamesAsync(List<string> unsortedNames, CancellationToken cancellationToken = default);
}