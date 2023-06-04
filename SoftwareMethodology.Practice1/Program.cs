using SoftwareMethodology.Practice1.Domain;

var parser = new InputParser();

try
{
    var testCases = parser.ParseTestCases();
    foreach (var testCase in testCases)
    {
        Console.WriteLine($"Case Number {testCase.Number}");
        var grid = new Map(testCase.Countries.ToList());
        Console.Write(grid.RunSimulation());
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
