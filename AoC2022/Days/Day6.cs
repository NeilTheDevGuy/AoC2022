namespace AoC2022.Days;

public static class Day6
{
    public static async Task Run()
    {
        var input = await InputGetter.GetAllAsString(6);
        Console.WriteLine($"Part One - {GetMessageStart(4, input)}");
        Console.WriteLine($"Part Two - {GetMessageStart(14, input)}");
    }

    private static int GetMessageStart(int matchingCharCount, string input)
    {
        for (int i = 0; i < input.Length - matchingCharCount; i++)
        {
            if (input.Substring(i, matchingCharCount).Distinct().Count() == matchingCharCount)            
                return i + matchingCharCount;
            
        }
        return default;
    }
}
