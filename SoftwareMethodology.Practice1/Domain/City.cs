namespace SoftwareMethodology.Practice1.Domain;

public class City
{
    private const int INITIAL_HOME_COUNTRY_COINS_COUNT = 1000000;
    private const int REPRESENTATIVE_PORTION_SIZE = 1000;
    private IReadOnlyCollection<City> _neighbours;
    private IReadOnlyCollection<CoinBalance> _balances;
    private readonly int _xCoordinate;
    private readonly int _yCoordinate;
    private readonly string _countryName;

    public City(string countryName, int xCoordinate, int yCoordinate)
    {
        _countryName = countryName;
        _xCoordinate = xCoordinate;
        _yCoordinate = yCoordinate;
    }

    public bool IsNeigbouringCity(City city)
    {
        var xWiseCoordinatesDifference = Math.Abs(_xCoordinate - city._xCoordinate);
        var yWiseCoordinatesDifference = Math.Abs(_yCoordinate - city._yCoordinate);

        return (xWiseCoordinatesDifference == 1 && yWiseCoordinatesDifference == 0) ||
               (xWiseCoordinatesDifference == 0 && yWiseCoordinatesDifference == 1);
    }

    public void AttachNeighbours(IReadOnlyCollection<City> neighbourCities) => _neighbours = neighbourCities;

    public void InitializeCoinBalances(IReadOnlyCollection<string> neighbouringCountryMotifs) 
        => _balances = neighbouringCountryMotifs.Select(ComposeInitialBalance).ToList();

    public void TransportCoins()
    {
        foreach (var neighbour in _neighbours)
        {
            foreach (var balance in _balances)
            {
                var representativePortion = balance.Amount / REPRESENTATIVE_PORTION_SIZE;
                neighbour.ReceiveCoins(representativePortion, balance.CountryMotif);
                balance.Decrease(representativePortion);
            }
        }
    }

    public void ReceiveCoins(int amount, string countryMotif)
    {
        var targetBalance = _balances.Single(b => b.CountryMotif == countryMotif);
        targetBalance.Increase(amount);
    }

    public void CommitBalanceTransactions()
    {
        foreach (var balance in _balances)
        {
            balance.CommitTransaction();
        }
    }

    public bool IsComplete() => _balances.All(c => c.Amount > 0);

    public bool SharesBordersWithAnotherCountry() => _neighbours.Any(n => n._countryName != _countryName);

    private CoinBalance ComposeInitialBalance(string cityCountry)
    {
        var initialCoinsCount = _countryName == cityCountry ? INITIAL_HOME_COUNTRY_COINS_COUNT : 0;

        return new CoinBalance(cityCountry, initialCoinsCount);
    }
}

