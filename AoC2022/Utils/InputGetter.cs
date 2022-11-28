namespace AoC2022.Utils;

public static class InputGetter
{
    public static async Task<long[]> GetFromCsvAsLong(int day)
    {
        var input = await File.ReadAllTextAsync(GetFileName(day));

        return input
            .Split(",")
            .Select(long.Parse)
            .ToArray();
    }

    public static async Task<string[]> GetFromCsvAsString(int day)
    {
        var input = await File.ReadAllTextAsync(GetFileName(day));
        return input
            .Split(",")
            .ToArray();
    }

    public static async Task<string[]> GetFromLinesAsString(int day)
    {
        return await File.ReadAllLinesAsync(GetFileName(day));
    }

    public static async Task<long[]> GetFromLinesAsLong(int day)
    {
        var stringLines = await File.ReadAllLinesAsync(GetFileName(day));
        return stringLines
            .Select(long.Parse)
            .ToArray();
    }

    public static async Task<string> GetAllAsString(int day)
    {
        return await File.ReadAllTextAsync(GetFileName(day));
    }


    private static string GetFileName(int day) => $@"Input/Day{day}.txt";
}
