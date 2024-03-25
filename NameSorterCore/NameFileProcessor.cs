using NameSorterCore.Models;
using NameSorterCore.Interfaces;

namespace NameSorterCore;

/// <summary>
/// Processes text files for sorting names according to defined criteria.
/// </summary>
public class NameFileProcessor : IFileProcessor
{
    private readonly INameSorterService _nameSorterService;
    private readonly IFileSystem _fileSystem;
    private readonly string _outputFilePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="NameFileProcessor"/> class.
    /// </summary>
    /// <param name="nameSorterService">The service to sort names.</param>
    /// <param name="fileSystem">The filesystem handler for file operations.</param>
    /// <param name="logger">Logger for logging messages.</param>
    /// <param name="appSettings">Application settings containing configurations such as file paths.</param>
    public NameFileProcessor(INameSorterService nameSorterService, IFileSystem fileSystem, AppSettings appSettings)
    {
        _nameSorterService = nameSorterService ?? throw new ArgumentNullException(nameof(nameSorterService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _outputFilePath = appSettings?.OutputFilePath ?? throw new ArgumentNullException(nameof(appSettings.OutputFilePath));
    }

    /// <summary>
    /// Processes the file at the given path, sorting the names found within according to the configured sorting strategy.
    /// </summary>
    /// <param name="filePath">The path to the file containing unsorted names.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!_fileSystem.FileExists(filePath))
        {
            Console.WriteLine("File does not exist.");
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            //1- Use FileHandler to read the unsorted names
            var unsortedNames = await _fileSystem.ReadAllLinesAsync(filePath, cancellationToken);
            if (!unsortedNames.Any())
            {
                Console.WriteLine("No names found to sort.");
                return;
            }

            //2- Sort & validate the names
            var sortedNames = await _nameSorterService.SortNamesAsync(unsortedNames, cancellationToken);
            if (!sortedNames.Any())
            {
                Console.WriteLine("No valid names found to sort after format validation.");
                return;
            }


            //4- write the sorted names to a file
            // Convert sorted PersonName objects back to string format
            List<string> sortedNamesString = sortedNames.Select(name => name.ToString()).ToList();
            await _fileSystem.WriteAllLinesAsync(_outputFilePath, sortedNamesString, cancellationToken);

            Console.WriteLine($"The names have been sorted and saved to '{_outputFilePath}'.");

            // Print sorted names to screen
            sortedNames.ForEach(name => Console.WriteLine(name.ToString()));

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was canceled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
