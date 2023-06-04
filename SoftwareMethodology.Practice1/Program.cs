using SoftwareMethodology.Practice1.Domain;

var parser = new InputParser();

try
{
    var testCases = parser.ParseTestCases();
    foreach (var testCase in testCases)
    {
        Console.WriteLine($"Case Number {testCase.Number}");
        try
        {
            var grid = new Map(testCase.Countries.ToList());
            Console.Write(grid.RunSimulation());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
