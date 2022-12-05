using System.Formats.Asn1;

namespace AoC2022.Days;

public static class Day3
{

    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(3);
        await PartOne(input);
        await PartTwo(input);
    }

    private static async Task PartOne(string[] input)
    {
        var totalPriorty = 0;
        foreach (var line in input)
        {
            var partOne = line.Substring(0, line.Length / 2);
            var partTwo = line.Substring(line.Length / 2);

            foreach (var letter in partOne)
            {
                if (partTwo.Contains(letter, StringComparison.CurrentCulture))
                {
                    totalPriorty += GetPriority(letter);
                    break;
                }
            }
        }
        Console.WriteLine($"Part One - {totalPriorty}");
    }

    private static async Task PartTwo(string[] input)
    {
        var totalPriorty = 0;
        for (int i = 0; i < input.Length; i+=3)
        {
            var rucksacks = input.Skip(i).Take(3);
            foreach (var letter in rucksacks.First())
            {
                var others = rucksacks.Where(x => x != rucksacks.First()).ToList();

                if (others.All(x => x.Contains(letter, StringComparison.CurrentCulture)))
                {
                    Console.WriteLine(letter);
                    totalPriorty += GetPriority(letter);
                    break;
                }
            }
        }
        Console.WriteLine($"Part Two - {totalPriorty}");
    }

    private static int GetPriority(char letter)
    {
        var asciiChar = (int)letter;
        if (asciiChar >= 65 && asciiChar <= 90)
        {
            return asciiChar - 38;
        }
        return asciiChar - 96;
    }

}
