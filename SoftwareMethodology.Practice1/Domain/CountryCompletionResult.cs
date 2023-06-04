using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareMethodology.Practice1.Domain;

public class CountryCompletionResult
{
    public bool IsComplete { get; private set; } = false;
    public int CompletedAtDays { get; private set; } = 0;

    public void MarkAsComplete(int days)
    {
        IsComplete = true;
        CompletedAtDays = days;
    }
}

