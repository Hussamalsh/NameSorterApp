using NameSorterCore.Models;

namespace NameSorterCore.Interfaces;

public interface INameParser
{
    PersonName Parse(string name);
}
