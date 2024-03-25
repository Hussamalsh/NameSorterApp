using Moq;
using NameSorterCore.Interfaces;
using NameSorterCore.Models;
using NameSorterCore.Sorters;

namespace NameSorterCore.Tests.Sorters;

[TestFixture]
public class NameSorterTests
{
    private NameSorter _nameSorter;
    private Mock<ISortStrategy<PersonName>> _mockSortStrategy;
    private Mock<INameParser> _mockNameParser;

    [SetUp]
    public void SetUp()
    {
        _mockSortStrategy = new Mock<ISortStrategy<PersonName>>();
        _mockNameParser = new Mock<INameParser>();

        _nameSorter = new NameSorter(_mockSortStrategy.Object, _mockNameParser.Object);
    }

    [Test]
    public void Sort_WithValidNames_CallsSortStrategyWithParsedNames()
    {
        // Setup
        var names = new List<string> { "Jane Doe", "John Smith" };
        var parsedNames = names.Select(n =>
        {
            var parts = n.Split(' ');
            return new PersonName(parts.Last(), parts.Take(parts.Length - 1).ToList());
        }).ToList();

        parsedNames.ForEach(parsedName =>
            _mockNameParser.Setup(p => p.Parse(It.Is<string>(s => names.Contains(s))))
                           .Returns(parsedName));

        _mockSortStrategy.Setup(s => s.Sort(It.IsAny<IEnumerable<PersonName>>()))
                         .Returns(parsedNames);

        // Act
        _nameSorter.Sort(names);

        // Assert
        _mockSortStrategy.Verify(s => s.Sort(It.IsAny<IEnumerable<PersonName>>()), Times.Once);
        names.ForEach(name => _mockNameParser.Verify(p => p.Parse(name), Times.Once));
    }

    [Test]
    public void Sort_WithNullNames_ThrowsArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => _nameSorter.Sort(null));
    }

    [Test]
    public void Sort_WithEmptyNamesList_ReturnsEmptyCollection()
    {
        // Setup
        var names = new List<string>();
        _mockSortStrategy.Setup(s => s.Sort(It.IsAny<IEnumerable<PersonName>>()))
                         .Returns(new List<PersonName>());

        // Act
        var result = _nameSorter.Sort(names);

        // Assert
        Assert.IsEmpty(result);
        _mockSortStrategy.Verify(s => s.Sort(It.IsAny<IEnumerable<PersonName>>()), Times.Once);
    }

    [Test]
    public void Sort_DelegatesToSortStrategy_CorrectlyParsesNames()
    {
        // Setup
        var names = new List<string> { "John Doe", "Jane Smith Hussam" };
        var parsedNames = new List<PersonName>
        {
            new PersonName("Doe", new List<string> { "John" }),
            new PersonName("Hussam", new List<string> { "Jane", "Smith" })
        };

        names.Zip(parsedNames, (n, parsed) => new { Name = n, Parsed = parsed })
            .ToList()
            .ForEach(item => _mockNameParser.Setup(p => p.Parse(item.Name))
                                            .Returns(item.Parsed));

        _mockSortStrategy.Setup(s => s.Sort(It.IsAny<IEnumerable<PersonName>>()))
                         .Returns(parsedNames);

        // Act
        var result = _nameSorter.Sort(names).ToList();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Any(n => n.LastName == "Doe" && n.GivenNames.Contains("John")));
        Assert.IsTrue(result.Any(n => n.LastName == "Hussam" && n.GivenNames.SequenceEqual(new List<string> { "Jane", "Smith" })));

        names.ForEach(name => _mockNameParser.Verify(p => p.Parse(name), Times.Once));
    }
}
