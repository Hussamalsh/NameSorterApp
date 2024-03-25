using NameSorterCore.Interfaces;
using NameSorterCore.Models;

namespace NameSorterCore.Validators;

public class NameParser : INameParser
{
    /// <summary>
    /// Parses a full name string into a PersonName object.
    /// </summary>
    /// <param name="name">The full name to parse.</param>
    /// <returns>A PersonName object.</returns>
    /// <exception cref="ArgumentException">Thrown when the name is invalid.</exception>
    public PersonName Parse(string name)
    {
        // Ensure the input is not null, empty, or whitespace.
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be null, empty, or whitespace.", nameof(name));
        }

        var parts = name.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Ensure there is at least one given name and one last name.
        if (parts.Length < 2)
        {
            throw new ArgumentException("Invalid name format. A name must consist of at least one given name and a last name.", nameof(name));
        }

        // Detect and reject names with invalid parts.
        if (parts.Any(part => string.IsNullOrWhiteSpace(part)))
        {
            throw new ArgumentException("Invalid name part detected. Each part of the name must be non-empty.", nameof(name));
        }

        // Separate the last name from the given names. Assumes the last part is the last name.
        var lastName = parts.Last();
        var givenNames = parts.Take(parts.Length - 1).ToList(); // Convert to List<string> as required by PersonName constructor.

        return new PersonName(lastName, givenNames);
    }
}
