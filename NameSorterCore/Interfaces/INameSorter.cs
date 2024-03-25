using NameSorterCore.Models;

namespace NameSorterCore.Interfaces;

public interface INameSorter
{
    List<PersonName> Sort(List<string> names);
}
