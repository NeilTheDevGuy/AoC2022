namespace AoC2022.Days;

public static class Day4
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(4);
        await PartOne(input);
        await PartTwo(input);
    }

    private static async Task PartOne(string[] input)
    {
        var pairs = 0;
        foreach (var line in input) 
        {
            (var section1, var section2) = GetSections(line);
            (var section1Min, var section1Max, var section2Min, var section2Max) = GetSectionMinAndMax(section1, section2);

            if ((section1Min >= section2Min && section1Max <= section2Max)
                || (section2Min >= section1Min && section2Max <= section1Max))
            {
                pairs++;
            }
        }
        Console.WriteLine($"Part 1 - {pairs}");
    }

    private static async Task PartTwo(string[] input)
    {
        var pairs = 0;
        foreach (var line in input)
        {
            (var section1, var section2) = GetSections(line);
            (var section1Min, var section1Max, var section2Min, var section2Max) = GetSectionMinAndMax(section1, section2);

            var range1 = Enumerable.Range(section1Min, (section1Max - section1Min) + 1).ToList();
            var range2 = Enumerable.Range(section2Min, (section2Max - section2Min) + 1).ToList();
            var combined = range1.Union(range2);
            var isDup = combined.Distinct().Count() < (range1.Count + range2.Count);

            if (isDup)
            {
                pairs++;
            }
        }
        Console.WriteLine($"Part 2 - {pairs}");
    }

    private static (string, string) GetSections(string line)
    {
        var section1 = line.Split(",")[0];
        var section2 = line.Split(",")[1];

        return (section1, section2);
    }

    private static (int, int, int, int) GetSectionMinAndMax(string section1, string section2)
    {
        var section1Min = int.Parse(section1.Split("-")[0]);
        var section1Max = int.Parse(section1.Split("-")[1]);
        var section2Min = int.Parse(section2.Split("-")[0]);
        var section2Max = int.Parse(section2.Split("-")[1]);

        return (section1Min, section1Max, section2Min, section2Max);
    }
}
