namespace NameSorterCore.Interfaces;

public interface IFileSystem
{
    bool FileExists(string path);
    Task<List<string>> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default);
    Task WriteAllLinesAsync(string outputPath, List<string> lines, CancellationToken cancellationToken = default);
}
