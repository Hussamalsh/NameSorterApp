using Moq;
using NameSorterCore.Interfaces;
using NameSorterCore.Models;
using NameSorterCore.Services;

namespace NameSorterCore.Tests.Services;

[TestFixture]
public class NameSorterServiceTests
{
    private Mock<INameValidator> _mockNameValidator;
    private Mock<INameSorter> _mockNameSorter;
    private NameSorterService _nameSorterService;

    [SetUp]
    public void SetUp()
    {
        _mockNameValidator = new Mock<INameValidator>();
        _mockNameSorter = new Mock<INameSorter>();
        _nameSorterService = new NameSorterService(_mockNameValidator.Object, _mockNameSorter.Object);
    }

    [Test]
    public async Task SortNamesAsync_WithValidNames_ReturnsSortedNames()
    {
        var unsortedNames = new List<string> { "John Doe", "Jane Smith" };
        var validNames = new List<string> { "John Doe", "Jane Smith" }; // Assuming all names are valid for this test
        var sortedNames = new List<PersonName> { new PersonName("Doe", new List<string> { "John" }), new PersonName("Smith", new List<string> { "Jane" }) };

        _mockNameValidator.Setup(v => v.ValidateNames(unsortedNames)).Returns(validNames);
        _mockNameSorter.Setup(s => s.Sort(validNames)).Returns(sortedNames);

        var result = await _nameSorterService.SortNamesAsync(unsortedNames);

        Assert.AreEqual(sortedNames, result);
        _mockNameValidator.Verify(v => v.ValidateNames(unsortedNames), Times.Once);
        _mockNameSorter.Verify(s => s.Sort(validNames), Times.Once);
    }

    [Test]
    public async Task SortNamesAsync_WithInvalidNames_FiltersAndSortsValidNamesOnly()
    {
        var unsortedNames = new List<string> { "Invalid Name", "Jane Smith" };
        var validNames = new List<string> { "Jane Smith" }; // Only "Jane Smith" is considered valid for this test
        var sortedNames = new List<PersonName> { new PersonName("Smith", new List<string> { "Jane" }) };

        _mockNameValidator.Setup(v => v.ValidateNames(unsortedNames)).Returns(validNames);
        _mockNameSorter.Setup(s => s.Sort(validNames)).Returns(sortedNames);

        var result = await _nameSorterService.SortNamesAsync(unsortedNames);

        Assert.AreEqual(sortedNames, result);
        _mockNameValidator.Verify(v => v.ValidateNames(unsortedNames), Times.Once);
        _mockNameSorter.Verify(s => s.Sort(validNames), Times.Once);
    }

    [Test]
    public async Task SortNamesAsync_WithEmptyList_ReturnsEmptyList()
    {
        var unsortedNames = new List<string>();

        // Setup the mocks to return an empty list if needed, or not setup if method should naturally handle empty lists
        var result = await _nameSorterService.SortNamesAsync(unsortedNames);

        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }


    [Test]
    public void SortNamesAsync_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _nameSorterService.SortNamesAsync(null));
    }

}
