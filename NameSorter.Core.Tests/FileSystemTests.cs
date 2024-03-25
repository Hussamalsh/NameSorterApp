
namespace NameSorterCore.Tests;

[TestFixture]
public class FileSystemTests
{

    private string _tempFilePath;
    private FileSystem _fileSystem;

    [SetUp]
    public void SetUp()
    {
        _fileSystem = new FileSystem();
        _tempFilePath = Path.GetTempFileName(); // Automatically creates the file
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath); // Cleanup
        }
    }


    [Test]
    public void FileExists_ExistingFile_ReturnsTrue()
    {
        //Act
        var exists = _fileSystem.FileExists(_tempFilePath);
        //Assert
        Assert.IsTrue(exists);
    }

    [Test]
    public void FileExists_NonExistingFile_ReturnsFalse()
    {
        var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        var exists = _fileSystem.FileExists(filePath);

        Assert.IsFalse(exists);
    }

    [Test]
    public async Task ReadAllLinesAsync_ExistingFile_ReturnsContents()
    {
        // Setup is now handled by [SetUp]
        var expectedLines = new List<string> { "Line 1", "Line 2" };
        await File.WriteAllLinesAsync(_tempFilePath, expectedLines);

        var lines = await _fileSystem.ReadAllLinesAsync(_tempFilePath);

        CollectionAssert.AreEqual(expectedLines, lines);
    }


    [Test]
    public void ReadAllLinesAsync_NonExistingFile_ThrowsApplicationException()
    {
        var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await _fileSystem.ReadAllLinesAsync(filePath));
        Assert.That(ex.Message, Does.StartWith("An I/O error occurred while reading the file:"));
    }

    [Test]
    public async Task WriteAllLinesAsync_ValidPath_WritesContents()
    {
        // Setup is now handled by [SetUp]
        var linesToWrite = new List<string> { "Test Line 1", "Test Line 2" };

        await _fileSystem.WriteAllLinesAsync(_tempFilePath, linesToWrite);

        var writtenLines = await File.ReadAllLinesAsync(_tempFilePath);
        CollectionAssert.AreEqual(linesToWrite, writtenLines);
    }

    [Test]
    public void WriteAllLinesAsync_InvalidPath_ThrowsApplicationException()
    {
        // Assuming you're testing on an environment where you don't have write permissions
        var filePath = "/invalidpath/testfile.txt";
        var linesToWrite = new List<string> { "Test Line" };

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await _fileSystem.WriteAllLinesAsync(filePath, linesToWrite));
        Assert.That(ex.Message, Does.StartWith("An error occurred while writing to the file:"));
    }

    [Test]
    public void ReadAllLinesAsync_OperationCanceled_ThrowsOperationCanceledException()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var filePath = Path.GetTempFileName(); 

        Assert.ThrowsAsync<TaskCanceledException>(async () => { await _fileSystem.ReadAllLinesAsync(filePath, cancellationTokenSource.Token); });
    }

    [Test]
    public async Task WriteAllLinesAsync_OperationCanceled_ThrowsOperationCanceledException()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var linesToWrite = new List<string> { "Test Line 1", "Test Line 2" };

        Assert.ThrowsAsync<TaskCanceledException>(async () => { await _fileSystem.WriteAllLinesAsync(_tempFilePath, linesToWrite, cancellationTokenSource.Token); });
    }



}
