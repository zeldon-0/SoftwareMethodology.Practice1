using System.Text;
using SoftwareMethodology.Practice1.Domain;

public class Map
{
    private const int MinCoordinateValue = 1;
    private const int MaxWidth = 10;
    private const int MaxLength = 10;
    private const int MaxCountryNameLength = 25;

    private List<City> Cities = new();
    public List<Country> Countries = new();

    public Map(IReadOnlyCollection<Country> countries)
    {
        foreach (var country in countries)
        {
            AttachCountry(country);
        }

        AttachNeighbouringCities();
        InitializeCityCoinBalances();
    }

    public string RunSimulation()
    {
        var currentDay = 0;

        foreach (var incompleteCountry in Countries.Where(country => !country.IsComplete()))
        {
            incompleteCountry.RefreshCompletionResult(currentDay);
        }

        while (Countries.Any(country => !country.IsComplete()))
        {
            currentDay++;
            RunTransactionsForTheDay();
            foreach (var incompleteCountry in Countries.Where(country => !country.IsComplete()))
            {
                incompleteCountry.RefreshCompletionResult(currentDay);
            }
        }

        var stringBuilder = new StringBuilder();

        foreach (var country in Countries
                     .OrderBy(country => country.GetCompletionTime())
                     .ThenBy(country => country.Name))
        {
            stringBuilder.Append($"{country.Name} {country.GetCompletionTime()}\n");
        }

        return stringBuilder.ToString();
    }

    private void AttachCountry(Country country)
    {
        if (country.Xl < MinCoordinateValue || country.Xl > MaxWidth ||
            country.Xh < MinCoordinateValue || country.Xh > MaxWidth ||
            country.Yl < MinCoordinateValue || country.Yl > MaxLength ||
            country.Yh < MinCoordinateValue || country.Yh > MaxLength)
        {
            throw new ArgumentException($"The X coordinates of a country should be within the range of {MinCoordinateValue} to {MaxWidth}," +
                                        $"and the Y coordinates of a country should be within the range of {MinCoordinateValue} to {MaxLength}." +
                                        $"{country.Name} failed the validation");
        }

        if (country.Xl > country.Xh || country.Yl > country.Yh)
        {
            throw new ArgumentException(
                "The lower left edge's coordinates should not have a higher absolute value than the top right ones. " +
                $"{country.Name} failed the validation");
        }

        if (country.Name.Length > MaxCountryNameLength)
        {
            throw new ArgumentException($"Expected the country name to be no longer than {MaxCountryNameLength} characters. Got {country.Name.Length}, instead.");
        }

        var validCities = new List<City>();
        for (var x = country.Xl; x <= country.Xh; x++)
        {
            for (var y = country.Yl; y <= country.Yh; y++)
            {
                if (Countries.Any(intersectingCountry => intersectingCountry.TerritoryIncludesCoordinates(x, y)))
                    throw new ArgumentException("Two or more countries' territories are overlapping. Check your input and try again.");

                validCities.Add(new City(country.Name, x, y));
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
                .Where(currentCity => currentCity.IsNeigbouringCity(city))
                .ToList();

            city.AttachNeighbours(neighbours);
        }
    }

    private void InitializeCityCoinBalances()
    {
        var countriesWithNeighbours = Countries
            .Where(country => country.Cities.Any(ct => ct.SharesBordersWithAnotherCountry()))
            .Select(country => country.Name)
            .ToList();

        foreach (var country in Countries.Where(country => countriesWithNeighbours.Contains(country.Name)))
        {
            foreach (var city in country.Cities)
            {
                city.InitializeCoinBalances(countriesWithNeighbours);
            }
        }

        foreach (var country in Countries.Where(country => !countriesWithNeighbours.Contains(country.Name)))
        {
            foreach (var city in country.Cities)
            {
                city.InitializeCoinBalances(new List<string> {country.Name});
            }
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

