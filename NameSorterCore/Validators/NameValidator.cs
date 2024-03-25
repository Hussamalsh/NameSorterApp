using NameSorterCore.Interfaces;

namespace NameSorterCore.Validators;

/// <summary>
/// Provides functionality for validating names based on specified formatting rules.
/// </summary>
public class NameValidator : INameValidator
{
    /// <summary>
    /// Validates a list of names, filtering out those that do not meet the validity criteria.
    /// </summary>
    /// <param name="names">The list of names to validate.</param>
    /// <returns>A list containing only the names that meet the validity criteria.</returns>
    /// <remarks>
    /// This method utilizes LINQ to apply the <see cref="IsValidNameFormat"/> method to each name in the input list,
    /// effectively filtering out invalid names.
    /// </remarks>
    public List<string> ValidateNames(List<string> names)
    {
        if (names == null)
        {
            throw new ArgumentNullException(nameof(names), "The list of names cannot be null.");
        }

        // Use LINQ to filter out names that do not meet the validity criteria
        var validNames = names.Where(IsValidNameFormat).ToList();
        return validNames;
    }

    /// <summary>
    /// Determines whether a given name string meets the specified format criteria for validity.
    /// </summary>
    /// <param name="name">The name to check for validity.</param>
    /// <returns><c>true</c> if the name meets the format criteria; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// A valid name is defined as having at least one given name and a last name, but no more than three given names,
    /// resulting in a total of 1 to 4 parts when the name is split by spaces.
    /// This method also ensures that each part of the name is not an empty string or whitespace.
    /// </remarks>
    public bool IsValidNameFormat(string name)
    {
        var parts = name.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // A valid name must have at least 1 given name and up to 3 given names (1-4 parts including the last name)
        return parts.Length >= 2 && parts.Length <= 4;
    }
}