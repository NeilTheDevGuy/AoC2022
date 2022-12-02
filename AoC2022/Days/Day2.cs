namespace AoC2022.Days;

public static class Day2
{
    const int RockScore = 1;
    const int PaperScore = 2;
    const int ScissorsScore = 3;

    const int WinScore = 6;
    const int DrawScore = 3;
    const int LoseScore = 0;

    const char Rock = 'X';
    const char Paper = 'Y';
    const char Scissors = 'Z';

    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(2);
        await PartOne(input);
        await PartTwo(input);
    }

    private static async Task PartOne(string[] input)
    {
        var score = 0;

        foreach (var line in input)
        {
            score += GetScore(line[0], line[2]);
        }

        Console.WriteLine($"Part 1 - {score}");
    }

    private static async Task PartTwo(string[] input)
    {
        var score = 0;

        foreach (var line in input)
        {
            var roundOutcome = GetRoundOutcome(line[0], line[2]);
            score += GetScore(line[0], roundOutcome);
        }

        Console.WriteLine($"Part 2 - {score}");
    }

    private static int GetScore(char opponent, char me)
    {
        return opponent switch
        {
            'A' when me == 'X' => RockScore + DrawScore,
            'A' when me == 'Y' => PaperScore + WinScore,
            'A' when me == 'Z' => ScissorsScore + LoseScore,
            'B' when me == 'X' => RockScore + + LoseScore,
            'B' when me == 'Y' => PaperScore + DrawScore,
            'B' when me == 'Z' => ScissorsScore + WinScore,
            'C' when me == 'X' => RockScore + WinScore,
            'C' when me == 'Y' => PaperScore + LoseScore,
            'C' when me == 'Z' => ScissorsScore + DrawScore,
        };
    }

    private static char GetRoundOutcome(char opponent, char me)
    {
        return me switch
        {
            'X' when opponent == 'A' => Scissors,
            'Y' when opponent == 'A' => Rock,
            'Z' when opponent == 'A' => Paper,
            'X' when opponent == 'B' => Rock,
            'Y' when opponent == 'B' => Paper,
            'Z' when opponent == 'B' => Scissors,
            'X' when opponent == 'C' => Paper,
            'Y' when opponent == 'C' => Scissors,
            'Z' when opponent == 'C' => Rock,
        };
    }
}
