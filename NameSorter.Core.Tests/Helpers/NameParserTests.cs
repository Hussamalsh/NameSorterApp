namespace NameSorterCore.Tests.Helpers;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using NameSorterCore.Validators;


[TestFixture]
public class NameParserTests
{
    private NameParser _nameParser;

    [SetUp]
    public void Setup()
    {
        _nameParser = new NameParser();
    }

    [Test]
    public void Parse_ValidName_ReturnsCorrectPersonNameObject()
    {
        var inputName = "John Michael Doe";
        var expectedLastName = "Doe";
        var expectedGivenNames = new List<string> { "John", "Michael" };

        var result = _nameParser.Parse(inputName);

        Assert.AreEqual(expectedLastName, result.LastName);
        CollectionAssert.AreEqual(expectedGivenNames, result.GivenNames);
    }

    [Test]
    public void Parse_OnlyLastName_ThrowsArgumentException()
    {
        var inputName = "Doe";

        var ex = Assert.Throws<ArgumentException>(() => _nameParser.Parse(inputName));
        Assert.That(ex.Message, Is.EqualTo("Invalid name format. A name must consist of at least one given name and a last name. (Parameter 'name')"));
    }

    [Test]
    public void Parse_EmptyString_ThrowsArgumentException()
    {
        var inputName = "";

        var ex = Assert.Throws<ArgumentException>(() => _nameParser.Parse(inputName));
        Assert.That(ex.Message, Is.EqualTo("Name must not be null, empty, or whitespace. (Parameter 'name')"));
    }

    [Test]
    public void Parse_NullString_ThrowsArgumentException()
    {
        string inputName = null;

        var ex = Assert.Throws<ArgumentException>(() => _nameParser.Parse(inputName));
        Assert.That(ex.Message, Is.EqualTo("Name must not be null, empty, or whitespace. (Parameter 'name')"));
    }

    [Test]
    public void Parse_WhitespaceString_ThrowsArgumentException()
    {
        var inputName = "   ";

        var ex = Assert.Throws<ArgumentException>(() => _nameParser.Parse(inputName));
        Assert.That(ex.Message, Is.EqualTo("Name must not be null, empty, or whitespace. (Parameter 'name')"));
    }

    [Test]
    public void Parse_NameWithExtraSpaces_ReturnsCorrectlyFormattedPersonName()
    {
        var inputName = "  John   Doe  ";
        var expectedLastName = "Doe";
        var expectedGivenNames = new List<string> { "John" };

        var result = _nameParser.Parse(inputName);

        Assert.AreEqual(expectedLastName, result.LastName);
        CollectionAssert.AreEqual(expectedGivenNames, result.GivenNames);
    }
}

