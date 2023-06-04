using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareMethodology.Practice1.Domain;
public class TestCase
{
    public int Number { get; private set; }
    public IReadOnlyCollection<Country> Countries { get; private set; }

    public TestCase(int number, IReadOnlyCollection<Country> countries)
    {
        Number = number;
        Countries = countries;
    }
}

