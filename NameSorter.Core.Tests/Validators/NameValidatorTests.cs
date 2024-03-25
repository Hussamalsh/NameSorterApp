using NameSorterCore.Validators;

namespace NameSorterCore.Tests.Validators;

[TestFixture]
public class NameValidatorTests
{
    private NameValidator _nameValidator;

    [SetUp]
    public void SetUp()
    {
        _nameValidator = new NameValidator();
    }

    [Test]
    public void ValidateNames_WithValidNames_ReturnsAllValidNames()
    {
        var names = new List<string> { "John Doe", "Jane Mary Smith", "Alex Jr. Doe" };
        var validNames = _nameValidator.ValidateNames(names);

        Assert.AreEqual(names.Count, validNames.Count);
        CollectionAssert.AreEquivalent(names, validNames);
    }

    [Test]
    public void ValidateNames_WithInvalidNames_FiltersOutInvalidNames()
    {
        var names = new List<string> { "John", "Doe", "John Doe", "John Edward Doe", "John Edward Martin Doe", "John Edward Martin Samuel Doe" };
        var expectedValidNames = new List<string> { "John Doe", "John Edward Doe", "John Edward Martin Doe" };
        var validNames = _nameValidator.ValidateNames(names);

        Assert.AreEqual(expectedValidNames.Count, validNames.Count);
        CollectionAssert.AreEquivalent(expectedValidNames, validNames);
    }

    [Test]
    public void ValidateNames_WithExcessiveSpaces_ReturnsValidNames()
    {
        var names = new List<string> { "John  Doe", "  Jane    Mary   Smith  " };
        var expectedValidNames = new List<string> { "John  Doe", "  Jane    Mary   Smith  " };
        var validNames = _nameValidator.ValidateNames(names);

        CollectionAssert.AreEquivalent(expectedValidNames, validNames);
    }

    [Test]
    public void ValidateNames_WithEmptyAndWhitespaceNames_FiltersOutInvalidNames()
    {
        var names = new List<string> { "", " ", "John Doe" };
        var expectedValidNames = new List<string> { "John Doe" };
        var validNames = _nameValidator.ValidateNames(names);

        Assert.AreEqual(expectedValidNames.Count, validNames.Count);
        CollectionAssert.AreEquivalent(expectedValidNames, validNames);
    }

    [Test]
    public void ValidateNames_WithNamesHavingMoreThanAllowedParts_FiltersOutInvalidNames()
    {
        var names = new List<string> { "John Edward Martin Samuel Doe", "Jane Doe", "John Doe Edward" };
        var expectedValidNames = new List<string> { "Jane Doe", "John Doe Edward" };
        var validNames = _nameValidator.ValidateNames(names);

        CollectionAssert.AreEquivalent(expectedValidNames, validNames);
    }

    [Test]
    public void ValidateNames_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _nameValidator.ValidateNames(null));
    }

    [Test]
    [TestCase("John Doe", ExpectedResult = true)]
    [TestCase("John", ExpectedResult = false)]
    [TestCase("John Edward Martin Doe", ExpectedResult = true)]
    [TestCase("John Edward Martin Samuel Doe", ExpectedResult = false)]
    [TestCase("  ", ExpectedResult = false)]
    [TestCase("", ExpectedResult = false)]
    public bool IsValidNameFormat_GivenVariousInputs_ReturnsExpectedResult(string name)
    {
        return _nameValidator.IsValidNameFormat(name);
    }



}
