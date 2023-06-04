namespace SoftwareMethodology.Practice1.Domain;
public class InputParser
{
    private const string INPUT_FILE_PATH = @"C:\Personal\Uni\SoftwareMethodology\SoftwareMethodology.Practice1\SoftwareMethodology.Practice1\Data\input.txt";

    public IReadOnlyCollection<TestCase> ParseTestCases()
    {
        var testCases = new List<TestCase>();
        List<string> input = ReadFile();
        
        var parsedTestCases = SplitFileByTestCases(input);
        var currentTestCaseNumber = 1;
        foreach (var testCase in parsedTestCases)
        {
            var testCaseCountries = new List<Country>();
            foreach (var countryLine in testCase)
            {
                var args = countryLine.Split(" ");
                if (args.Length != 5)
                    throw new ArgumentException($"A country definition should include its name and the four coordinates. Found only {args.Length} arguments.");
               
                var name = args[0];
                var xl = int.Parse(args[1]);
                var yl = int.Parse(args[2]);
                var xh = int.Parse(args[3]);
                var yh = int.Parse(args[4]);
                testCaseCountries.Add(new Country(name, xl, yl, xh, yh));
            }
            testCases.Add(new TestCase(currentTestCaseNumber, testCaseCountries));
            currentTestCaseNumber++;
        }

        return testCases;
    }

    private List<string> ReadFile()
    {
        using StreamReader file = new StreamReader(INPUT_FILE_PATH);
        string line;
        var result = new List<string>();
        while ((line = file.ReadLine()) != null)
        {
            result.Add(line);
        }

        return result;
    }

    private IReadOnlyCollection<IReadOnlyCollection<string>> SplitFileByTestCases(List<string> lines)
    {
        var initialCountryNumberLine = lines.First().Trim();
        var initialCountryNumber = int.Parse(initialCountryNumberLine);

        var result = new List<List<string>>();
        var unparsedLines = lines.Skip(1).ToList();
        result.Add(unparsedLines.Take(initialCountryNumber).ToList());
        unparsedLines = unparsedLines.Skip(initialCountryNumber).ToList();

        while (unparsedLines.Count > 1)
        {
            var countryNumberLine = unparsedLines.First().Trim();
            var nextCountryNumber = int.Parse(countryNumberLine);
            unparsedLines = unparsedLines.Skip(1).ToList();
            result.Add(unparsedLines.Take(nextCountryNumber).ToList());
            unparsedLines = unparsedLines.Skip(nextCountryNumber).ToList();
        }

        if (unparsedLines.Count != 1)
            throw new ArgumentException("Invalid input file structure!");

        var lastLineParsed = int.TryParse(unparsedLines.Single(), out var endOfFileValue);

        if (!lastLineParsed || endOfFileValue != 0)
            throw new ArgumentException("The input file must end in a line with the '0' symbol.");

        return result;
    }
}

