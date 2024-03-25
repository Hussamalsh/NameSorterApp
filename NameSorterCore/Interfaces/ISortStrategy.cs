namespace NameSorterCore.Interfaces;

public interface ISortStrategy<T>
{
    IEnumerable<T> Sort(IEnumerable<T> items);
}

