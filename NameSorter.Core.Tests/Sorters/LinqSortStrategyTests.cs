using NameSorterCore.Models;
using NameSorterCore.Sorters;

namespace NameSorterCore.Tests.Sorters;

[TestFixture]
public class LinqSortStrategyTests
{
    private LinqSortStrategy<PersonName> _linqSortStrategy;

    [SetUp]
    public void SetUp()
    {
        // Setup the LinqSortStrategy with PersonName specific sorting criteria
        _linqSortStrategy = new LinqSortStrategy<PersonName>(
            primarySortCriteria: name => name.LastName,
            secondarySortCriteria: name => string.Join(" ", name.GivenNames));
    }

    [Test]
    public void Sort_WithMultipleNames_SortsCorrectlyByLastNameThenGivenNames()
    {
        var names = new List<PersonName>
        {
            new PersonName("Doe", new List<string> { "John" }),
            new PersonName("Smith", new List<string> { "Jane" }),
            new PersonName("Doe", new List<string> { "Jane" })
        };

        var sortedNames = _linqSortStrategy.Sort(names).ToList();

        Assert.AreEqual("Doe", sortedNames[0].LastName);
        Assert.AreEqual("Jane", sortedNames[0].GivenNames.First());
        Assert.AreEqual("Doe", sortedNames[1].LastName);
        Assert.AreEqual("John", sortedNames[1].GivenNames.First());
        Assert.AreEqual("Smith", sortedNames[2].LastName);
    }

    [Test]
    public void Sort_WithSameLastNames_SortsFurtherByGivenNames()
    {
        var names = new List<PersonName>
        {
            new PersonName("Smith", new List<string> { "Zoe" }),
            new PersonName("Smith", new List<string> { "Amy" })
        };

        var sortedNames = _linqSortStrategy.Sort(names).ToList();

        Assert.AreEqual("Amy", sortedNames[0].GivenNames.First());
        Assert.AreEqual("Zoe", sortedNames[1].GivenNames.First());
    }

    [Test]
    public void Sort_WithEmptyList_ReturnsEmptyCollection()
    {
        var names = new List<PersonName>();

        var sortedNames = _linqSortStrategy.Sort(names).ToList();

        Assert.IsEmpty(sortedNames);
    }

    [Test]
    public void Sort_WithSingleName_ReturnsSingleNameList()
    {
        var names = new List<PersonName> { new PersonName("Doe", new List<string> { "John" }) };

        var sortedNames = _linqSortStrategy.Sort(names).ToList();

        Assert.AreEqual(1, sortedNames.Count);
        Assert.AreEqual("Doe", sortedNames[0].LastName);
        Assert.AreEqual("John", sortedNames[0].GivenNames.First());
    }
}
