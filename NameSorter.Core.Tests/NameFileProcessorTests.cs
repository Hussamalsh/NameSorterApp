using Moq;
using NameSorterCore.Interfaces;
using NameSorterCore.Models;
using NameSorterCore.Tests.Helpers;

namespace NameSorterCore.Tests;

[TestFixture]
public class NameFileProcessorTests
{
    private Mock<IFileSystem> _mockFileSystem;
    private Mock<INameSorterService> _mockNameSorterService;
    private AppSettings _appSettings;
    private NameFileProcessor _nameFileProcessor;

    [SetUp]
    public void Setup()
    {
        _mockFileSystem = new Mock<IFileSystem>();
        _mockNameSorterService = new Mock<INameSorterService>();

        // Corrected to match the expected return type of SortNamesAsync
        _mockNameSorterService.Setup(s => s.SortNamesAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new List<PersonName>()); // Return an empty list of PersonName to simulate no valid names found

        _appSettings = new AppSettings { OutputFilePath = "sorted-names-list.txt" };

        _nameFileProcessor = new NameFileProcessor(_mockNameSorterService.Object, _mockFileSystem.Object, _appSettings);
    }

    [Test]
    public async Task ProcessFileAsync_WhenFileExistsAndContainsValidNames_SortsAndWritesNamesToFile()
    {
        // Arrange
        var unsortedNames = new List<string> { "Jane Doe", "John Smith" };
        var sortedNames = new List<PersonName> {
        new PersonName("Doe", new List<string> { "Jane" }),
        new PersonName("Smith", new List<string> { "John" })
    };

        ArrangeFileSystemMocks(unsortedNames);
        _mockNameSorterService.Setup(s => s.SortNamesAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(sortedNames);

        // Act
        await _nameFileProcessor.ProcessFileAsync("validnames.txt");

        // Assert
        _mockFileSystem.Verify(fs => fs.WriteAllLinesAsync(
            _appSettings.OutputFilePath,
            It.Is<List<string>>(names => names.SequenceEqual(new[] { "Jane Doe", "John Smith" })),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ProcessFileAsync_InvalidNamesFound_PrintsNoValidNamesMessage()
    {
        ArrangeFileSystemMocks(new List<string> { "Invalid Name" });

        using (var consoleOutput = new ConsoleOutput())
        {
            await _nameFileProcessor.ProcessFileAsync("invalidnames.txt");
            Assert.That(consoleOutput.GetOutput(), Contains.Substring("No valid names found to sort after format validation."));
        }
    }

    [Test]
    public async Task ProcessFileAsync_FileDoesNotExist_PrintsErrorMessage()
    {
        _mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(false);

        using (var consoleOutput = new ConsoleOutput())
        {
            await _nameFileProcessor.ProcessFileAsync("nonexistentfile.txt");
            Assert.That(consoleOutput.GetOutput(), Is.EqualTo($"File does not exist.{Environment.NewLine}"));
        }
    }

    [Test]
    public async Task ProcessFileAsync_FileIsEmpty_PrintsNoNamesFoundMessage()
    {
        ArrangeFileSystemMocks(new List<string>());

        var textProcessor = new NameFileProcessor(_mockNameSorterService.Object, _mockFileSystem.Object, _appSettings);
        using (var consoleOutput = new ConsoleOutput())
        {
            await textProcessor.ProcessFileAsync("emptyfile.txt");
            Assert.That(consoleOutput.GetOutput(), Contains.Substring("No names found to sort."));
        }
    }

    [Test]
    public async Task ProcessFileAsync_ReadFileError_PrintsErrorMessage()
    {
        _mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
        _mockFileSystem.Setup(fs => fs.ReadAllLinesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new IOException("Read error"));

        using (var consoleOutput = new ConsoleOutput())
        {
            await _nameFileProcessor.ProcessFileAsync("errorfile.txt");
            Assert.That(consoleOutput.GetOutput(), Contains.Substring("An error occurred: Read error"));
        }
    }

    [Test]
    public async Task ProcessFileAsync_WriteFileError_PrintsErrorMessage()
    {
        _mockFileSystem.Setup(fs => fs.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new IOException("Write error"));

        using (var consoleOutput = new ConsoleOutput())
        {
            await _nameFileProcessor.ProcessFileAsync("writablefile.txt");
            Assert.That(consoleOutput.GetOutput(), Contains.Substring("File does not exist."));
        }
    }


    [Test]
    public async Task ProcessFileAsync_OperationCanceled_ThrowsOperationCanceledException()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        _mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);

        Assert.ThrowsAsync<OperationCanceledException>(async () => {
            await _nameFileProcessor.ProcessFileAsync("validnames.txt", cancellationTokenSource.Token);
        });
    }

    [Test]
    public async Task ProcessFileAsync_ValidNames_PrintsSortedNamesToConsole()
    {
        // Setup the file system mock to simulate a file exists and returns some names
        ArrangeFileSystemMocks(new List<string> { "John Doe", "Jane Smith" });
        // Correctly setup the name sorter service to return a sorted list of names
        var sortedNames = new List<PersonName> {
        new PersonName("Doe", new List<string> { "John" }),
        new PersonName("Smith", new List<string> { "Jane" })
    };
        _mockNameSorterService.Setup(s => s.SortNamesAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(sortedNames); // Ensure this line is correctly setup

        using (var consoleOutput = new ConsoleOutput())
        {
            await _nameFileProcessor.ProcessFileAsync("namesfile.txt");

            // Verify that sorted names are printed to the console
            var output = consoleOutput.GetOutput();
            Assert.That(output, Contains.Substring("John Doe"));
            Assert.That(output, Contains.Substring("Jane Smith"));
        }
    }


    private void ArrangeFileSystemMocks(List<string> fileContent)
    {
        _mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
        _mockFileSystem.Setup(fs => fs.ReadAllLinesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(fileContent);
    }

}