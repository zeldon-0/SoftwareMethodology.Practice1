using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareMethodology.Practice1.Domain;

public class Country
{
    public string Name { get; }
    public int Xl { get; }
    public int Yl { get; }
    public int Xh { get; }
    public int Yh { get; }
    public IReadOnlyCollection<City> Cities { get; private set; }
    private readonly CountryCompletionResult _completionResult;

    public Country(string name, int xl, int yl, int xh, int yh)
    {
        Name = name;
        Xl = xl;
        Yl = yl;
        Xh = xh;
        Yh = yh;
        _completionResult = new CountryCompletionResult();
    }
    public bool TerritoryIncludesCoordinates(int xCoordinate, int yCoordinate) =>
        xCoordinate >= Xl && xCoordinate <= Xh && yCoordinate >= Yl && yCoordinate <= Yh;

    public void AttachCities(IReadOnlyCollection<City> cities) => Cities = cities;

    public void RefreshCompletionResult(int currentDay)
    {
        if (Cities.All(c => c.IsComplete()))
        {
            _completionResult.MarkAsComplete(currentDay);
        }
    }

    public bool IsComplete() => _completionResult.IsComplete;

    public int GetCompletionTime() => _completionResult.CompletedAtDays;
}

