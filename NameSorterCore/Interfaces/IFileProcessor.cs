namespace NameSorterCore.Interfaces;

public interface IFileProcessor
{
    Task ProcessFileAsync(string filePath, CancellationToken cancellationToken = default);
}
