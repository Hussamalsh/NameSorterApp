namespace NameSorterCore.Models;

public class PersonName
{
    public string LastName { get; set; }
    public List<string> GivenNames { get; set; }

    public PersonName(string lastName, List<string> givenNames)
    {
        LastName = lastName;
        GivenNames = givenNames;
    }

    public override string ToString()
    {
        return $"{string.Join(" ", GivenNames)} {LastName}".Trim();
    }
}
