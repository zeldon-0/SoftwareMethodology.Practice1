using System.Text;
using SoftwareMethodology.Practice1.Domain;

public class Map
{
    private const int Width = 10;
    private const int Length = 10;

    private List<City> Cities = new();
    public List<Country> Countries = new();

    public Map(IReadOnlyCollection<Country> countries)
    {
        foreach (var country in countries)
        {
            AttachCountry(country, countries);
        }

        AttachNeighbouringCities();
    }

    public string RunSimulation()
    {
        var currentDay = 0;

        foreach (var country in Countries)
        {
            country.RefreshCompletionResult(currentDay);
        }

        while (Countries.Any(c => !c.IsComplete()))
        {
            currentDay++;
            RunTransactionsForTheDay();
            foreach (var incompleteCountry in Countries.Where(c => !c.IsComplete()))
            {
                incompleteCountry.RefreshCompletionResult(currentDay);
            }
        }

        var stringBuilder = new StringBuilder();

        foreach (var country in Countries
                     .OrderBy(c => c.GetCompletionTime())
                     .ThenBy(c => c.Name))
        {
            stringBuilder.Append($"{country.Name} {country.GetCompletionTime()}\n");
        }

        return stringBuilder.ToString();
    }

    private void AttachCountry(Country country, IReadOnlyCollection<Country> allCountries)
    {
        if (country.Xl < 1 || country.Xl > Width ||
            country.Xh < 1 || country.Xh > Width ||
            country.Yl < 1 || country.Yl > Length ||
            country.Yh < 1 || country.Yh > Length)
            throw new ArgumentException("The coordinates of a country should be within the range of 1 to 10. " +
                                        $"{country.Name} failed the validation");

        if (country.Xl > country.Xh || country.Yl > country.Yh)
            throw new ArgumentException("The lower left edge's coordinates should not have a higher absolute value than the top right ones. " +
                                        $"{country.Name} failed the validation");

        var validCities = new List<City>();
        var allCountryNames = allCountries.Select(c => c.Name).ToList();
        for (var x = country.Xl; x <= country.Xh; x++)
        {
            for (var y = country.Yl; y <= country.Yh; y++)
            {
                if (Countries.Any(c => c.TerritoryIncludesCoordinates(x, y)))
                    throw new ArgumentException("Two or more countries' territories are overlapping. Check your input and try again.");

                validCities.Add(new City(allCountryNames, country.Name, x, y));
            }
        }

        country.AttachCities(validCities);
        Countries.Add(country);
        Cities.AddRange(validCities);
    }

    private void AttachNeighbouringCities()
    {
        foreach (var city in Cities)
        {
            var neighbours = Cities
                .Where(c => c.IsNeigbouringCity(city))
                .ToList();

            city.AttachNeighbours(neighbours);
        }
    }

    private void RunTransactionsForTheDay()
    {
        foreach (var city in Cities)
        {
            city.TransportCoins();
        }
        foreach (var city in Cities)
        {
            city.CommitBalanceTransactions();
        }
    }
}

