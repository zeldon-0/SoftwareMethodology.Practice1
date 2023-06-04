using FluentAssertions;
using SoftwareMethodology.Practice1.Domain;

namespace SoftwareMethodology.Practice1.Tests;
public class MapTests
{
    [Theory]
    [MemberData(nameof(RunSimulationData))]
    public void RunSimulation_should_return_expected_output_string(
        IReadOnlyCollection<Country> countries,
        string expectedOutput)
    {
        // Arrange
        var map = new Map(countries);
        
        // Act
        var actualOutput = map.RunSimulation();

        // Assert
        actualOutput.Should().Be(expectedOutput);
    }


    public static IEnumerable<object[]> RunSimulationData()
    {
        yield return new object[]
        {
            new List<Country>
            {
                new("France", 1, 4, 4, 6),
                new("Spain", 3, 1, 6, 3),
                new("Portugal", 1, 1, 2, 2)
            },
            "Spain 382\n" +
            "Portugal 416\n" +
            "France 1325\n"
        };
        yield return new object[]
        {
            new List<Country>
            {
                new("Luxembourg", 1, 1, 1, 1)
            },
            "Luxembourg 0\n"
        };
        yield return new object[]
        {
            new List<Country>
            {
                new("Netherlands", 1, 3, 2, 4),
                new("Belgium", 1, 1, 2, 2)
            },
            "Belgium 2\n" +
            "Netherlands 2\n"
        };
    }
}
