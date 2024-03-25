using NameSorterCore.Interfaces;

namespace NameSorterCore;

/// <summary>
/// Provides an implementation of <see cref="IFileSystem"/> for interacting with the file system.
/// </summary>
public class FileSystem : IFileSystem
{
    /// <summary>
    /// Checks if a file exists at the specified path.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
    public bool FileExists(string path) => File.Exists(path);

    /// <summary>
    /// Asynchronously reads all lines from the file at the specified path.
    /// </summary>
    /// <param name="inputPath">The path to the file.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation, containing a list of strings read from the file.</returns>
    /// <exception cref="ApplicationException">Thrown when an I/O error occurs or an unexpected error occurs while reading the file.</exception>
    public async Task<List<string>> ReadAllLinesAsync(string inputPath, CancellationToken cancellationToken = default)
    {
        try
        {
            var lines = await File.ReadAllLinesAsync(inputPath, cancellationToken);
            return lines.ToList();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (IOException ex)
        {
            throw new ApplicationException($"An I/O error occurred while reading the file: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An unexpected error occurred while reading the file: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Asynchronously writes a list of strings to the file at the specified path.
    /// </summary>
    /// <param name="outputPath">The path where the file will be written.</param>
    /// <param name="lines">The list of strings to write to the file.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="ApplicationException">Thrown when an error occurs while writing to the file.</exception>
    public async Task WriteAllLinesAsync(string outputPath, List<string> lines, CancellationToken cancellationToken = default)
    {
        try
        {
            await File.WriteAllLinesAsync(outputPath, lines, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while writing to the file: {ex.Message}", ex);
        }
    }


}
